using Shared.DDD;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Models
{
   [Table("todo", Schema = "public")]
    public class Todo : Entity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("is_complete")]
        public bool IsComplete { get; set; }
    }
}