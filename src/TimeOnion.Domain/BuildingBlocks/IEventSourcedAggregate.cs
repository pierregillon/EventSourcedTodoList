using System.Reflection;

namespace TimeOnion.Domain.BuildingBlocks;

public interface IEventSourcedAggregate
{
    IEnumerable<IDomainEvent> UncommittedChanges { get; }
    void LoadFromHistory(IReadOnlyCollection<IDomainEvent> domainEvents);
    void MarkAsCommitted();

    public static TAggregate Rehydrate<TAggregate, TAggregateId>(
        TAggregateId aggregateId,
        IReadOnlyCollection<IDomainEvent> eventHistory
    )
        where TAggregate : IEventSourcedAggregate
    {
        var aggregate = CreateNewInstance<TAggregate, TAggregateId>(aggregateId);

        aggregate.LoadFromHistory(eventHistory);

        return aggregate;
    }

    private static TAggregate CreateNewInstance<TAggregate, TAggregateId>(TAggregateId id)
    {
        var constructorInfo = typeof(TAggregate).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic,
                new[] { typeof(TAggregateId) }
            )
            ?? throw new InvalidOperationException($"Cannot find constructor for {typeof(TAggregate).Name.ToLower()}");

        var aggregate = (TAggregate)constructorInfo.Invoke(new object?[] { id })
            ?? throw new InvalidOperationException(
                $"Fail to instanciate the aggregate {typeof(TAggregate).Name.ToLower()}");

        return aggregate;
    }
}