namespace WebApplication8.DI.Services.ErorServices
{
    public class ErorHandler : IErorHandler
    {
        public async Task SendJsonEror(HttpContext context,string message)
        {
            var JsonEror = new {
                Type = "Eror",
                StatusCode = context.Response.StatusCode,
                ServerMessage = message,
                Time = DateTime.Now,
            };
           await context.Response.WriteAsJsonAsync(JsonEror);
        }
    }
}
