using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApplication8.Entity
{
    public class User
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public int Age { get; set; }
        public override string ToString()
        {
            return new JsonObject
            {
                ["Id"] = Id,
                ["Name"] = Name,
                ["Surname"] = Surname,
                ["Age"] = Age
            }.ToString();
        }
    }
}
