using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Domain.Todo.List;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private readonly List<TodoListItem> _items = new();

    private TodoList(TodoListId id) : base(id)
    {
    }

    public static TodoList Empty => new(TodoListId.Unique);

    public void AddItem(ItemDescription description) => StoreEvent(new TodoItemAdded(TodoItemId.New(), description));

    public void MarkItemAsDone(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null) throw new InvalidOperationException("Cannot complete the item: unknown item");

        StoreEvent(new TodoItemCompleted(Id, itemId));
    }

    public void MarkItemAsToDo(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null) throw new InvalidOperationException("Cannot mark the item as to do: unknown item");

        StoreEvent(new ItemReadyTodo(Id, item.Id));
    }

    public void FixItemDescription(TodoItemId itemId, ItemDescription newItemDescription)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null) throw new InvalidOperationException("Cannot fix item description: unknown item");

        StoreEvent(new TodoItemDescriptionFixed(Id, item.Id, item.Description, newItemDescription));
    }

    public static TodoList Rehydrate(IEnumerable<IDomainEvent> eventHistory)
    {
        var todoList = Empty;

        foreach (var domainEvent in eventHistory) todoList.Apply(domainEvent);

        return todoList;
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case TodoItemAdded added:
                _items.Add(new TodoListItem(added.ItemId, added.Description));
                break;
        }
    }

    private record TodoListItem(TodoItemId Id, ItemDescription Description);
}

public record TodoItemDescriptionFixed(TodoListId Id, TodoItemId ItemId, ItemDescription PreviousItemDescription,
    ItemDescription NewItemDescription) : IDomainEvent;

public record ItemReadyTodo(TodoListId Id, TodoItemId ItemId) : IDomainEvent;

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new TodoItemId(Guid.NewGuid());
}

public record TodoItemAdded(TodoItemId ItemId, ItemDescription Description) : IDomainEvent;

public record TodoItemCompleted(TodoListId TodoListId, TodoItemId TodoItemId) : IDomainEvent;