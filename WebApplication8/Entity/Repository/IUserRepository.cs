using WebApplication8.DI;

namespace WebApplication8.Entity.Repository
{
    public interface IUserRepository : IRepository<User,OperationStatus>
    {
        User? GetById(string Id);
        User? GetById(int Id);
        List<User> GetByLimit(int limit);
    }
}

