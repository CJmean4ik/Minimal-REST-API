using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public abstract class OptionsMiddleware<T>
    {
        private IUserRepository _usersRepository;
        private IErorHandler _erorHandler;
        private RequestDelegate _nextMiddleware;
        private ILogger<T> _logger;
        public IErorHandler ErorHandler { get => _erorHandler; set => _erorHandler = value; }
        public IUserRepository UsersRepository { get => _usersRepository; set => _usersRepository = value; }
        public RequestDelegate Next { get => _nextMiddleware; set => _nextMiddleware = value; }
        public ILogger<T> Logger { get => _logger; set => _logger = value; }

        public OptionsMiddleware(RequestDelegate nextMiddleware,
                                 IUserRepository usersRepository,
                                 IErorHandler erorHandler,
                                 ILogger<T> logger)
        {
            _usersRepository = usersRepository;
            _erorHandler = erorHandler;
            _nextMiddleware = nextMiddleware;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Logger.LogInformation("The client sent a request along the path: " + context.Request.Path);
            await ProccesingRequest(context);
        }

        public abstract Task ProccesingRequest(HttpContext context);
      
    }
}
