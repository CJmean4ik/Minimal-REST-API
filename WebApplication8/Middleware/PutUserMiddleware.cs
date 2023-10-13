using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class PutUserMiddleware : OptionsMiddleware
    {
        public PutUserMiddleware(RequestDelegate nextMiddleware, IUserRepository usersRepository, IErorHandler erorHandler) 
            : base(nextMiddleware, usersRepository, erorHandler)
        {
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            string requestPath = context.Request.Path;
            if (requestPath == "/api/v0.0.1/users/update" && context.Request.Method.ToLower() == "put")
            {
                var UserforUpdate = await context.Request.ReadFromJsonAsync<User>();
                var operationStatus = UsersRepository.Update(UserforUpdate);
                await context.Response.WriteAsJsonAsync(operationStatus);
                return;
            }
            await Next.Invoke(context);
        }
    }
}
