using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Infrastructure;

public class TodoListRepository : EventRepository<TodoListId, TodoList>, ITodoListRepository
{
    public TodoListRepository(IEventStore eventStore, IDomainEventPublisher domainEventPublisher) : base(eventStore,
        domainEventPublisher)
    {
    }
}