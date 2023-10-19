using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class PostUsersMiddleware : OptionsMiddleware<PostUsersMiddleware>
    {
        public PostUsersMiddleware(RequestDelegate nextMiddleware,
                                   IUserRepository usersRepository,
                                   IErorHandler erorHandler,
                                   ILogger<PostUsersMiddleware> logger)
                                   : base(nextMiddleware, usersRepository, erorHandler, logger)
        {
        }

        public override async Task ProccesingRequest(HttpContext context)
        {
            string requestPath = context.Request.Path;

            if (requestPath == "/api/v0.0.1/users/add" && context.Request.Method.ToLower() == "post")
            {
                User? user = await context.Request.ReadFromJsonAsync<User>();
                var operationResult = UsersRepository.Create(user);
                await context.Response.WriteAsJsonAsync(operationResult);
                return;
            }
            await Next.Invoke(context);
        }
    }
}
