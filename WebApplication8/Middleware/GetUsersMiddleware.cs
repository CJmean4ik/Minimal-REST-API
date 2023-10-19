using System.Collections.Generic;
using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class GetUsersMiddleware : OptionsMiddleware<GetUsersMiddleware>
    {
        public GetUsersMiddleware(RequestDelegate nextMiddleware,
                                  IUserRepository usersRepository,
                                  IErorHandler erorHandler,
                                  ILogger<GetUsersMiddleware> logger)
                                  : base(nextMiddleware, usersRepository, erorHandler, logger)
        { }

        public override async Task ProccesingRequest(HttpContext context)
        {
            string requestPath = context.Request.Path;

            if (requestPath == "/api/v0.0.1/users" && context.Request.Method.ToLower() == "get")
            {
                var result = await QueryProccesingForLimitParameterAsync(context);
                if (!result.done || result.haveEror) return;

                result = await QueryProccesingForIdParameterAsync(context);
                if (!result.done || result.haveEror) return;

                await SendAllUsersInResponceAsync(context);
                return;
            }
            await Next.Invoke(context);
        }
        private async Task<(bool done, bool haveEror)> QueryProccesingForLimitParameterAsync(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey("_limit")) return (false, false);

            Logger.LogInformation($"The client sent a request along the:" +
                "\npath: " + context.Request.Path +
                "\nquery: ?_limit=" + context.Request.Query["_limit"] +
                "\nTime: " + DateTime.Now);

            int limit = GetQueryStringByKeyName(context, "_limit");
            if (limit == 0)
            {
                Logger.LogWarning("Client sent not number in query path: " + limit + "Time: " + DateTime.Now);
                await ErorHandler.SendJsonEror(context, "Query parameter < _limit > must have number value");
                return (false, true);
            }

            await SendLimitUsersInResponceAsync(context, limit);
            return (true, true);

        }
        private async Task<(bool done, bool haveEror)> QueryProccesingForIdParameterAsync(HttpContext context)
        {
            if (!context.Request.Query.ContainsKey("id")) return (false, false);

                Logger.LogInformation($"The client sent a request along the:\npath: {context.Request.Path}\nquery: ?id={context.Request.Query["id"]}");

                int id = GetQueryStringByKeyName(context, "id");
                if (id == 0)
                {
                    Logger.LogWarning("Client sent not number for parametr {id} in query path: " + id + " Time: " + DateTime.Now);

                    await ErorHandler.SendJsonEror(context, "Query parameter < id > must have number value");
                    return (false, true);
                }

                await SendUsersByIdInResponceAsync(context, id.ToString());
                return (true, true);

        }
        private int GetQueryStringByKeyName(HttpContext httpContext, string keyName)
        {
            foreach (var queryString in httpContext.Request.Query)
            {
                if (queryString.Key == keyName)
                {
                    int.TryParse(queryString.Value, out int value);
                    return value;
                }
            }
            Logger.LogWarning("Key: " + keyName + " in httpContext.Request.Query was not found");
            httpContext.Response.StatusCode = 500;
            return 0;
        }
        private async Task SendLimitUsersInResponceAsync(HttpContext httpContext, int limit)
        {
            try
            {
                List<User> limitedUser = UsersRepository.GetByLimit(limit);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Headers.Add("x-total-count", limitedUser.Count.ToString());
                await httpContext.Response.WriteAsJsonAsync(limitedUser);
                Logger.LogInformation("Server sent " + limit + "users " + "Time: " + DateTime.Now);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message,"Time: " + DateTime.Now);
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync($"{ex.Message}");

            }
        }
        private async Task SendUsersByIdInResponceAsync(HttpContext httpContext, string id)
        {
            try
            {
                var idUser = UsersRepository.GetById(id);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.Headers.Add("x-total-count", "1");
                if (idUser == null)
                {

                    await ErorHandler.SendJsonEror(httpContext, $"User by id: {id} not found");
                    Logger.LogWarning("User by id: " + id + " was not found " + "Time: " + DateTime.Now);
                    return;
                }
                Logger.LogInformation("Server sent users by id: " + idUser.Id + " Time: " + DateTime.Now);
                await httpContext.Response.WriteAsJsonAsync(idUser);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, "Time: " + DateTime.Now);
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync($"{ex.Message}");
            }
        }
        private async Task SendAllUsersInResponceAsync(HttpContext context)
        {
            try
            {
                var users = UsersRepository.GetAll();
                context.Response.ContentType = "application/json";
                context.Response.Headers.Add("x-total-count", users.Count.ToString());
                Logger.LogInformation("Server sent all users. Count: " + users.Count + " Time: " + DateTime.Now);
                await context.Response.WriteAsJsonAsync(users);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, "Time: " + DateTime.Now);
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"{ex.Message}");
            }
        }
    }
}
