using Shared.DDD;
using Shared.Enums.Role;
using Shared.Enums.User;

namespace Shared.Messaging.Events
{
    public record CreateUserEvent(string LastName, string FirstName, string email, string Password, RoleType Role, UserStatus UserStatus) : IDomainEvent
    {
        //public string LastName { get; set; } = string.Empty;
        //public string FirstName { get; set; } = string.Empty;
        //public string Email { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        //public string Role { get; set; } = string.Empty;
        //public string UserStatus { get; set; } = string.Empty;
    }
}