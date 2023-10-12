using WebApplication8.DI;

namespace WebApplication8.Entity.Repository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users;

        public UserRepository()
        {
            _users = CreateDeffaultUsers();
        }

        public OperationStatus Create(User? entity)
        {
            if (entity == null) return 
                    new OperationStatusBuilder()
                    .CreateErrorStatus("Entity must dont have null", StatusName.Error);

            if (_users.Contains(entity)) return
                    new OperationStatusBuilder()
                  .CreateErrorStatus("Entity allredy existing in List<Users>", StatusName.Warning);


            string lastUserId = _users.Last().Id;
            int newUserId = int.Parse(lastUserId) + 1;
            entity.Id = newUserId.ToString();

            _users.Add(entity);
            return new OperationStatusBuilder().CreateSuccessfulStatusAdding(entity.ToString());
        }
        public OperationStatus Delete(User? entity)
        {
            if (entity == null) return
                    new OperationStatusBuilder()
                    .CreateErrorStatus("Entity must dont have null", StatusName.Error);

            if (!_users.Contains(entity)) return
                     new OperationStatusBuilder()
                   .CreateErrorStatus("Entity not exist in List<Users>", StatusName.Warning);

            _users.Remove(entity);
            return new OperationStatusBuilder().CreateSuccessfulStatusRemoving();
        }
        public List<User> GetAll()
        {
            return _users;
        }
        public User? GetById(string Id) => _users.Find(p => p.Id == Id);     
        public User? GetById(int Id) => GetById(Id.ToString());
        public List<User> GetByLimit(int limit) => _users.Take(limit).ToList();

        public OperationStatus Update(User? entity)
        {
            throw new NotImplementedException();
        }

        private List<User> CreateDeffaultUsers() => new List<User> 
        {
               new User { Id = "1", Name = "Stas", Surname = "ggggas", Age = 19 },
               new User { Id = "2", Name = "Slava", Surname = "Sdawd", Age = 19 },
               new User { Id = "3", Name = "Andrei", Surname = "Fasd1", Age = 19 },
               new User { Id = "4", Name = "Vania", Surname = "1rsdfs", Age = 19 }
        };        
    }
}
