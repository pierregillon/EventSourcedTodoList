using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Infrastructure;

public class TodoListRepository : ITodoListRepository
{
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly IEventStore _eventStore;

    public TodoListRepository(IEventStore eventStore, IDomainEventPublisher domainEventPublisher)
    {
        _eventStore = eventStore;
        _domainEventPublisher = domainEventPublisher;
    }

    public async Task<TodoList> Get(TodoListId id)
    {
        var eventHistory = await _eventStore.GetAll(id.Value);
        if (eventHistory.Count == 0)
        {
            throw new InvalidOperationException("The todo list could not be found.");
        }

        return TodoList.Rehydrate(id, eventHistory);
    }

    public async Task Save(TodoList aggregate)
    {
        await _eventStore.Save(aggregate.UncommittedChanges);
        await _domainEventPublisher.Publish(aggregate.UncommittedChanges);
        aggregate.MarkAsCommitted();
    }
}