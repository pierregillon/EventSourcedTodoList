using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Infrastructure;

public class TodoListRepository : ITodoListRepository
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IEventStore _eventStore;

    public TodoListRepository(IEventStore eventStore, IDomainEventPublisher domainEventPublisher)
    {
        _eventStore = eventStore;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<TodoList> Get()
    {
        var eventHistory = await _eventStore.GetAll();
        return TodoList.Rehydrate(eventHistory);
    }

    public async Task Save(TodoList aggregate)
    {
        await _eventStore.AddRange(aggregate.UncommittedChanges);
        await _domainEventPublisher.Publish(aggregate.UncommittedChanges);
        aggregate.MarkAsCommitted();
    }
}