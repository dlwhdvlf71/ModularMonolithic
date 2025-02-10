using Shared.DDD;
using Shared.Enums.Role;
using Shared.Enums.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.Models
{
    [Table("user", Schema = "public")]
    public class User : Entity
    {
        [Column("last_name")]
        public string LastName { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("role")]
        public RoleType Role { get; set; }

        [Column("user_status")]
        public UserStatus UserStatus { get; set; }
    }
}