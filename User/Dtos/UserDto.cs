﻿using Shared.Enums.Role;
using Shared.Enums.User;

namespace User.Dtos
{
    public record UserDto(string LastName, string FirstName, string Email, string Password, RoleType Role, UserStatus UserStatus);
}