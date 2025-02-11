using Shared.Enums.Role;
using Shared.Enums.User;

namespace User.Dtos
{
    public record UserDto(string LastName, string FirstName, string email, string Password, RoleType Role, UserStatus UserStatus);
}