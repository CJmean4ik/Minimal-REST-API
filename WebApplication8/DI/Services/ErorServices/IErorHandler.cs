namespace WebApplication8.DI.Services.ErorServices
{
    public interface IErorHandler
    {
        Task SendJsonEror(HttpContext context, string message);
    }
}
