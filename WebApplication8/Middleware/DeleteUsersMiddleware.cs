﻿using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public class DeleteUsersMiddleware : OptionsMiddleware
    {
        public DeleteUsersMiddleware(RequestDelegate nextMiddleware, IUserRepository usersRepository, IErorHandler erorHandler)
            : base(nextMiddleware, usersRepository, erorHandler)

        {
        }

        public override async Task InvokeAsync(HttpContext context)
        {
            string requestPath = context.Request.Path;
            if (requestPath == "/api/v0.0.1/users/remove" && context.Request.Method.ToLower() == "delete") 
            {
                var UserforDelete = await context.Request.ReadFromJsonAsync<List<User>>();
                var operationStatus =  UsersRepository.Delete(UserforDelete);
                await context.Response.WriteAsJsonAsync(operationStatus);
                return;
            }
            await Next.Invoke(context);
        }
    }
}