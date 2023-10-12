using System.Text.Json;

namespace WebApplication8.DI
{
    public class OperationStatusBuilder
    {
        private OperationStatus _operationStatus;

        public OperationStatusBuilder()
        {
            _operationStatus = new OperationStatus();
        }
        public OperationStatus CreateSuccessfulStatusAdding()
        {
            _operationStatus.OperationId = new Random().Next(0, 100);
            _operationStatus.Status = StatusName.Created;
            _operationStatus.Title = "Entity created and added";
            return _operationStatus;
        }
        public OperationStatus CreateSuccessfulStatusAdding(string body)
        {
            var middleResult = CreateSuccessfulStatusAdding();
            middleResult.JsonBody = JsonSerializer.SerializeToNode(body);
            return middleResult;
        }
        public OperationStatus CreateSuccessfulStatusRemoving()
        {
            _operationStatus.OperationId = new Random().Next(0, 100);
            _operationStatus.Status = StatusName.Deleted;
            _operationStatus.Title = "Entity has been deleted";
            return _operationStatus;
        }
        public OperationStatus CreateSuccessfulStatusUpdating()
        {
            _operationStatus.OperationId = new Random().Next(0, 100);
            _operationStatus.Status = StatusName.Updated;
            _operationStatus.Title = "Entity has been updating";
            return _operationStatus;
        }
        public OperationStatus CreateErrorStatus(string message,StatusName statusName)
        {
            _operationStatus.OperationId = new Random().Next(0, 100);
            _operationStatus.Status = statusName;
            _operationStatus.Title = message;
            return _operationStatus;
        }
    }
}
