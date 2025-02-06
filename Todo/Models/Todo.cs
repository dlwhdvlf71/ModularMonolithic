using Shared.DDD;

namespace Todo.Models
{
    public class Todo : Entity
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
}