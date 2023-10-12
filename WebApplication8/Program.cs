using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity.Repository;
using WebApplication8.Middleware;

namespace WebApplication8
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builderOptions = new WebApplicationOptions { Args = args };
            var builder = WebApplication.CreateBuilder(builderOptions);   
            
            builder.Services.AddSingleton<IUserRepository,UserRepository>();
            builder.Services.AddSingleton<IErorHandler,ErorHandler>();          
            builder.Services.AddCors();

            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            }

            app.UseMiddleware<GetUsersMiddleware>();
            app.UseMiddleware<PostUsersMiddleware>();
            app.UseMiddleware<DeleteUsersMiddleware>();
            app.UseMiddleware<PutUserMiddleware>();

            await app.RunAsync();
        }
    }
}