using Shared.Enums.Role;
using Shared.Enums.User;

namespace Shared.Messaging.Events
{
    public record CreatedUserSendIntegrationEvent : IntegrationEvent
    {
        public string LastName { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Password { get; set; } = default!;

        public RoleType Role { get; set; } = default!;

        public UserStatus UserStatus { get; set; } = default!;
    }
}