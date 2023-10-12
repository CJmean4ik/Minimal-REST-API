using WebApplication8.DI;

namespace WebApplication8.Entity.Repository
{
    public interface IRepository<T,R> 
        where T : class, new()
        where R : OperationStatus, new()
    {
        List<T> GetAll();
        R Create(T? entity);
        R Update(T? entity);
        R Delete(T? entity);
        List<R> Delete(List<T>? entitys);
    }
}
