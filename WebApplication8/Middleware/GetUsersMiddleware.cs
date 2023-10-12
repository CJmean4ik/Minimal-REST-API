using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class GetUsersMiddleware : OptionsMiddleware
    {
        public GetUsersMiddleware(RequestDelegate nextMiddleware, IUserRepository usersRepository, IErorHandler erorHandler) 
            : base(nextMiddleware, usersRepository, erorHandler)
        {
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            string requestPath = context.Request.Path;

            if (requestPath == "/api/v0.0.1/users" && context.Request.Method.ToLower() == "get")
            {
                if (context.Request.Query.ContainsKey("_limit"))
                {
                    int limit = GetQueryStringByKeyName(context, "_limit");
                    if (limit == 0)
                    {
                        context.Response.StatusCode = 400;
                        await ErorHandler.SendJsonEror(context, "Query parameter < _limit > must have number value");
                        return;
                    }

                    await SendLimitUsersInResponceAsync(context, limit);
                    return;
                }
                if (context.Request.Query.ContainsKey("id"))
                {
                    int id = GetQueryStringByKeyName(context, "id");
                    if (id == 0)
                    {
                        context.Response.StatusCode = 400;
                        await ErorHandler.SendJsonEror(context, "Query parameter < id > must have number value");
                        return;
                    }
                    await SendUsersByIdInResponceAsync(context, id.ToString());
                    return;
                }
                await SendAllUsersInResponceAsync(context);
            }
            await Next.Invoke(context);
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
            }
            catch (Exception ex)
            {
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
                }
                await httpContext.Response.WriteAsJsonAsync(idUser);
            }
            catch (Exception ex)
            {
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
                await context.Response.WriteAsJsonAsync(users);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync($"{ex.Message}");
            }
        }
    }
}
