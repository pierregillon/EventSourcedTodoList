using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement;

public record UserListProjection(IReadModelDatabase Database) : IDomainEventListener<UserRegistered>
{
    public async Task On(UserRegistered domainEvent) =>
        await Database.Add(new UserListItem(domainEvent.UserId, domainEvent.EmailAddress, domainEvent.PasswordHash));
}

public record UserListItem(UserId UserId, EmailAddress EmailAddress, PasswordHash PasswordHash);