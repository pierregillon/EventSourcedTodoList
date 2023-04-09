using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Infrastructure;

public abstract class EventRepository<TId, TAggregate> where TAggregate : EventSourcedAggregate<TId>
{
    private readonly IDomainEventPublisher _domainEventPublisher;

    public EventRepository(IDomainEventPublisher domainEventPublisher)
    {
        _domainEventPublisher = domainEventPublisher;
    }

    public Task<TodoList> Get()
    {
        return Task.FromResult(TodoList.Empty);
    }

    public async Task Save(TAggregate aggregate)
    {
        await _domainEventPublisher.Publish(aggregate.UncommittedChanges);
    }
}