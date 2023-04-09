using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Domain.Todo.List;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private TodoList(TodoListId id) : base(id)
    {
    }

    public static TodoList Empty => new(TodoListId.Unique);

    public void AddItem(ItemDescription description)
    {
        StoreEvent(new TodoItemAdded(description));
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
    }
}

public record TodoItemAdded(ItemDescription Description) : IDomainEvent;