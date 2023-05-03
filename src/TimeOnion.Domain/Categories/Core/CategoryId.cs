using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core;

public record CategoryId(Guid Value) : IAggregateId
{
    public static CategoryId New() => new(Guid.NewGuid());
    public static CategoryId? None => null;

    public static CategoryId From(string value) => new(Guid.Parse(value));
}