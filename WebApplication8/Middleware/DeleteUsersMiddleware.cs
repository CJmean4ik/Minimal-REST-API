using WebApplication8.DI;
using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class DeleteUsersMiddleware : OptionsMiddleware<DeleteUsersMiddleware>
    {
        public DeleteUsersMiddleware(RequestDelegate nextMiddleware,
                                     IUserRepository usersRepository,
                                     IErorHandler erorHandler,
                                     ILogger<DeleteUsersMiddleware> logger) 
                                     : base(nextMiddleware, usersRepository, erorHandler, logger)
        {
        }

        public override async Task ProccesingRequest(HttpContext context)
        {
            string requestPath = context.Request.Path;
            if (requestPath == "/api/v0.0.1/users/remove" && context.Request.Method.ToLower() == "delete") 
            {
                var UserforDelete = await context.Request.ReadFromJsonAsync<List<User>>();
                var operationResults = UsersRepository.Delete(UserforDelete);
                LogginRemovingUsers(operationResults);
                await context.Response.WriteAsJsonAsync(operationResults);
                return;
            }
            await Next.Invoke(context);
        }
        private void LogginRemovingUsers(List<OperationStatus>? operationStatuses)
        {
            if (operationStatuses == null) return;
           
            foreach (var operationStatus in operationStatuses)
            {
                Logger.LogInformation("User removing status:" +
                                      " Status: " + operationStatus.OperationId +
                                      " Status: " + operationStatus.Status +
                                      " Message: " + operationStatus.Title +
                                      " Time: " + DateTime.Now);
            }
        }
    }
}
