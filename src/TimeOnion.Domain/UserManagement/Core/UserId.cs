using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.UserManagement.Core;

public record UserId(Guid Value) : IAggregateId
{
    public static UserId New() => new(Guid.NewGuid());

    public static UserId Parse(string rawValue) => new(Guid.Parse(rawValue));
}