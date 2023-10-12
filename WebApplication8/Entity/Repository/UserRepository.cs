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
        public List<OperationStatus> Delete(List<User>? entitys)
        {
            var messages = new List<OperationStatus>();

            if (entitys == null || entitys.Count == 0)
            {
                messages.Add(new OperationStatusBuilder().CreateErrorStatus("List for delete entity must dont have null or exist entity", StatusName.Error));
                return messages;
            }

            foreach (var userForDelete in entitys)
            {
                var operationStatus = Delete(userForDelete);
                messages.Add(operationStatus);
            }
            return messages;
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
            if (entity == null) return
                    new OperationStatusBuilder()
                    .CreateErrorStatus("Entity must dont have null", StatusName.Error);

            if (!_users.Exists(ex => ex.Id == entity.Id)) return 
                    new OperationStatusBuilder()
                  .CreateErrorStatus($"List<Users> dont containt user by id: {entity.Id}", StatusName.Warning);


            OperationStatus operationStatus = new OperationStatus();

          var result = _users.Select(s => {

              if (s.Id == entity.Id)
              {
                 operationStatus = ChangeOldUserOnNew(s,entity);
                  return s;
              }
              return s;
          });

            return operationStatus;
        }
        private OperationStatus ChangeOldUserOnNew(User oldUser,User newUser)
        {
            if (oldUser.Name != newUser.Name) oldUser.Name = newUser.Name;

            if (oldUser.Surname != newUser.Surname) oldUser.Surname = newUser.Surname;

            if (oldUser.Age != newUser.Age) oldUser.Age = newUser.Age;

            return new OperationStatusBuilder().CreateSuccessfulStatusUpdating();
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
