using WebApplication8.DI;

namespace WebApplication8.Entity.Repository
{
    public class UserRepository : IUserRepository
    {
        private List<User> _users;
        private ILogger<UserRepository> _repositoryLogger;
        public UserRepository(ILogger<UserRepository> repositoryLogger)
        {
            _users = CreateDeffaultUsers();
            _repositoryLogger = repositoryLogger;
        }

        public OperationStatus Create(User? entity)
        {
            if (entity == null)
            {
                _repositoryLogger.LogWarning("Client send entity which contain null");
                return new OperationStatusBuilder()
                  .CreateErrorStatus("Entity must dont have null", StatusName.Warning);
            }

            if (_users.Contains(entity))
            {
                _repositoryLogger.LogWarning("Entity which cliend cend, allready contain");
                return new OperationStatusBuilder()
                  .CreateErrorStatus("Entity allredy existing in List<Users>", StatusName.Warning);
            }

                   


            string lastUserId = _users.Last().Id;
            int newUserId = int.Parse(lastUserId) + 1;
            entity.Id = newUserId.ToString();

            _users.Add(entity);

            _repositoryLogger.LogInformation("Client succsesful created entity " + entity.ToString());
            return new OperationStatusBuilder().CreateSuccessfulStatusAdding(entity.ToString());
        }
        public OperationStatus Delete(User? entity)
        {
            if (entity == null)
            {
                _repositoryLogger.LogWarning("Client send entity which contain null");
                return new OperationStatusBuilder()
                  .CreateErrorStatus("Entity must dont have null", StatusName.Warning);
            }


            if (!_users.Exists(ex => ex.Id == entity.Id))
            {
                _repositoryLogger.LogWarning("In List<User> has dont find a this users: " + entity.Id);
                return new OperationStatusBuilder()
                  .CreateErrorStatus("Entity not exist in List<Users>", StatusName.Warning);
            }
               


            var entityForDelete = _users.Find(f => f.Id == entity.Id)!;
            _users.Remove(entityForDelete);
            _repositoryLogger.LogInformation("Client succsesful removed entity " + entity.ToString());
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
            OperationStatus operationStatus = new OperationStatus();


            if (entity == null)
            {
                _repositoryLogger.LogWarning("Client send entity which contain null");
                return new OperationStatusBuilder()
                  .CreateErrorStatus("Entity must dont have null", StatusName.Warning);
            }


            if (!_users.Exists(ex => ex.Id == entity.Id)) return 
                    new OperationStatusBuilder()
                  .CreateErrorStatus($"List<Users> dont containt user by id: {entity.Id}", StatusName.Warning);


            foreach (var user in _users)
            {
                if (user.Id == entity.Id)
                {
                    operationStatus = ChangeOldUserOnNew(user,entity);
                }
            }

            return operationStatus;
        }
        private OperationStatus ChangeOldUserOnNew(User oldUser,User newUser)
        {
            bool ItChanged = false;

            if (oldUser.Name != newUser.Name)
            {
                oldUser.Name = newUser.Name;
                ItChanged = true;
                _repositoryLogger.LogInformation("A change was noticed for the property Users.name. New value: " + newUser.Name + " Old value: " + oldUser.Name);
            }

            if (oldUser.Surname != newUser.Surname)
            {
                oldUser.Surname = newUser.Surname;
                ItChanged = true;
                _repositoryLogger.LogInformation("A change was noticed for the property Users.Surname. New value: " + newUser.Surname + " Old value: " + oldUser.Surname);
            }

            if (oldUser.Age != newUser.Age)
            {
                oldUser.Age = newUser.Age;
                ItChanged = true;
                _repositoryLogger.LogInformation("A change was noticed for the property Users.Age. New value: " + newUser.Age + " Old value: " + oldUser.Age);
            }

            if (ItChanged) return new OperationStatusBuilder().CreateSuccessfulStatusUpdating();

            _repositoryLogger.LogInformation("Nothing has been changed in enityty");
            return new OperationStatusBuilder().CreateErrorStatus(" The user sent the data for changes, but these new data corresponded to the old ones. Changes not applied", StatusName.Warning);
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
