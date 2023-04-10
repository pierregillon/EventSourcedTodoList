using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Domain.Todo.List;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private readonly List<TodoListItem> _items = new();

    private TodoList(TodoListId id) : base(id)
    {
    }

    public static TodoList Empty => new(TodoListId.Unique);

    public void AddItem(ItemDescription description)
    {
        StoreEvent(new TodoItemAdded(TodoItemId.New(), description));
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case TodoItemAdded added:
                _items.Add(new TodoListItem(added.ItemId));
                break;
        }
    }

    public void Complete(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null) throw new InvalidOperationException("Cannot complete the item: unknown item");

        StoreEvent(new TodoItemCompleted(Id, itemId));
    }

    public static TodoList Rehydrate(IEnumerable<IDomainEvent> eventHistory)
    {
        var todoList = Empty;

        foreach (var domainEvent in eventHistory) todoList.Apply(domainEvent);

        return todoList;
    }

    private record TodoListItem(TodoItemId Id);
}

public record TodoItemId(Guid Value)
{
    public static TodoItemId New()
    {
        return new TodoItemId(Guid.NewGuid());
    }
}

public record TodoItemAdded(TodoItemId ItemId, ItemDescription Description) : IDomainEvent;

public record TodoItemCompleted(TodoListId TodoListId, TodoItemId TodoItemId) : IDomainEvent;