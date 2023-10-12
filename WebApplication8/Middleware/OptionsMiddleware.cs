using WebApplication8.DI.Services.ErorServices;
using WebApplication8.Entity.Repository;

namespace WebApplication8.Middleware
{
    public abstract class OptionsMiddleware
    {
        private IUserRepository _usersRepository;
        private IErorHandler _erorHandler;
        private RequestDelegate _nextMiddleware;
        public IErorHandler ErorHandler { get => _erorHandler; set => _erorHandler = value; }
        public IUserRepository UsersRepository { get => _usersRepository; set => _usersRepository = value; }
        public RequestDelegate Next { get => _nextMiddleware; set => _nextMiddleware = value; }

        public OptionsMiddleware(RequestDelegate nextMiddleware, IUserRepository usersRepository, IErorHandler erorHandler)
        {
            _usersRepository = usersRepository;
            _erorHandler = erorHandler;
            _nextMiddleware = nextMiddleware;
        }

        public abstract Task InvokeAsync(HttpContext context);       
    }
}
