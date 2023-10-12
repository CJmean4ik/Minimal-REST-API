using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class PostUsersMiddleware : OptionsMiddleware
    {
        public PostUsersMiddleware(RequestDelegate nextMiddleware, IUserRepository usersRepository, IErorHandler erorHandler) 
            : base(nextMiddleware, usersRepository, erorHandler)
        {
        }
        public override async Task InvokeAsync(HttpContext context)
        {
            string requestPath = context.Request.Path;

            if (requestPath == "/api/v0.0.1/users/add" && context.Request.Method.ToLower() == "post")
            {
                User? user = await context.Request.ReadFromJsonAsync<User>();
                var operationResult = UsersRepository.Create(user);
                if (operationResult.Status == DI.StatusName.Error || operationResult.Status == DI.StatusName.Warning)
                {
                    await context.Response.WriteAsJsonAsync(operationResult);
                    return;
                }
                await context.Response.WriteAsJsonAsync(operationResult);
                return;
            }

        }
    }
}
