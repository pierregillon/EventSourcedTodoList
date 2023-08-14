using System.Text.Json.Serialization;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IUserDomainEvent
{
    UserId UserId { get; set; }
}

public abstract record UserDomainEvent(
    Guid AggregateId,
    [property: JsonIgnore] UserId? InitialUserId = default
) : DomainEvent(AggregateId), IUserDomainEvent
{
    UserId IUserDomainEvent.UserId { get; set; } = InitialUserId!;
}