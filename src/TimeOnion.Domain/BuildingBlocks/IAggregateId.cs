namespace TimeOnion.Domain.BuildingBlocks;

public interface IAggregateId
{
    Guid Value { get; }
}