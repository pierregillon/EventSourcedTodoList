using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.UserManagement.Projections;

public record UserProjection(IReadModelDatabase Database) : IDomainEventListener<UserRegistered>
{
    public async Task On(UserRegistered domainEvent) =>
        await Database.Add(new UserProjectItem(domainEvent.UserId, domainEvent.EmailAddress, domainEvent.PasswordHash));
}

public record UserProjectItem(
    UserId UserId, 
    EmailAddress EmailAddress, 
    PasswordHash PasswordHash
) : IProjection;