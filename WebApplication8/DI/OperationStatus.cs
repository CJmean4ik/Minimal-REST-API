using System.Text.Json.Nodes;

namespace WebApplication8.DI
{
    public class OperationStatus
    {
        public int OperationId { get; set; }
        public StatusName Status { get; set; }
        public string? Title { get; set; }
        public JsonNode? JsonBody { get; set; }      
    }
    public enum StatusName
    {
        Created = 100,
        Deleted = 200,
        Updated = 300,
        Error = 001,
        Warning = 002
    }
}
