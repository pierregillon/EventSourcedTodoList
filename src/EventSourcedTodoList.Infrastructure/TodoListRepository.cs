using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Infrastructure;

public class TodoListRepository : EventRepository<TodoListId, TodoList>, ITodoListRepository
{
    public TodoListRepository(IDomainEventPublisher domainEventPublisher) : base(domainEventPublisher)
    {
    }
}