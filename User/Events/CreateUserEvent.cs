﻿using Shared.DDD;
using Shared.Enums.Role;
using Shared.Enums.User;

namespace User.Events
{
    public record CreateUserEvent(string LastName, string FirstName, string Email, string Password, RoleType Role, UserStatus UserStatus) : IDomainEvent;
}