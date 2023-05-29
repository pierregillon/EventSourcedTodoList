using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

public record UserRegistered
    (UserId UserId, EmailAddress EmailAddress, PasswordHash PasswordHash) : DomainEvent(UserId.Value);