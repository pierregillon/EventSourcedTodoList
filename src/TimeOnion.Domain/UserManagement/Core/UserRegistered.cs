using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

public record UserRegistered(
    UserId UserId,
    EmailAddress EmailAddress,
    PasswordHash PasswordHash
) : UserDomainEvent(UserId.Value, UserId);