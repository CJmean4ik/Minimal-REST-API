using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class PutUserMiddleware : OptionsMiddleware<PutUserMiddleware>
    {
        public PutUserMiddleware(RequestDelegate nextMiddleware, 
                                 IUserRepository usersRepository, 
                                 IErorHandler erorHandler,
                                 ILogger<PutUserMiddleware> logger)
                                 : base(nextMiddleware, usersRepository, erorHandler, logger)
        {
        }

        public override async Task ProccesingRequest(HttpContext context)
        {
            string requestPath = context.Request.Path;
            if (requestPath == "/api/v0.0.1/users/update" && context.Request.Method.ToLower() == "put")
            {
                var UserforUpdate = await context.Request.ReadFromJsonAsync<User>();
                var operationResult = UsersRepository.Update(UserforUpdate);
                Logger.LogInformation("User updating status:" +
                   " Status: " + operationResult.Status +
                   " Message: " + operationResult.Title +
                   " Time: " + DateTime.Now);
                await context.Response.WriteAsJsonAsync(operationResult);
                return;
            }
            return;
        }
    }
}
