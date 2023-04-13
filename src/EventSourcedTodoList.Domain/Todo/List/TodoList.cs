using EventSourcedTodoList.Domain.BuildingBlocks;

namespace EventSourcedTodoList.Domain.Todo.List;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private readonly List<TodoListItem> _items = new();

    private TodoList(TodoListId id) : base(id)
    {
    }

    public static TodoList Empty => new(TodoListId.Unique);

    public void AddItem(ItemDescription description, Temporality temporality) =>
        StoreEvent(new TodoItemAdded(TodoItemId.New(), description, temporality));

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

    public void Reschedule(TodoItemId itemId, Temporality anotherTemporality)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null) throw new InvalidOperationException("Cannot reschedule item to do: unknown item");

        if (item.Temporality != anotherTemporality)
            StoreEvent(new TodoItemRescheduled(Id, item.Id, item.Temporality, anotherTemporality));
    }

    public static TodoList Rehydrate(IEnumerable<IDomainEvent> eventHistory)
    {
        var todoList = Empty;

        foreach (var domainEvent in eventHistory) todoList.Apply(domainEvent);

        return todoList;
    }

    protected override void Apply(IDomainEvent domainEvent)
    {
        TodoListItem item;

        switch (domainEvent)
        {
            case TodoItemAdded added:
                _items.Add(new TodoListItem(added.ItemId, added.Description, added.Temporality));
                break;

            case TodoItemDescriptionFixed descriptionFixed:
                item = _items.Single(x => x.Id == descriptionFixed.ItemId);
                _items.Replace(item, item with { Description = descriptionFixed.NewItemDescription });
                break;

            case TodoItemRescheduled rescheduled:
                item = _items.Single(x => x.Id == rescheduled.ItemId);
                _items.Replace(item, item with { Temporality = rescheduled.NewTemporality });
                break;
        }
    }

    private record TodoListItem(TodoItemId Id, ItemDescription Description, Temporality Temporality);
}

public record TodoItemRescheduled(TodoListId Id, TodoItemId ItemId, Temporality PreviousTemporality,
    Temporality NewTemporality) : IDomainEvent;

public enum Temporality
{
    ThisDay,
    ThisWeek
}

public record TodoItemDescriptionFixed(TodoListId Id, TodoItemId ItemId, ItemDescription PreviousItemDescription,
    ItemDescription NewItemDescription) : IDomainEvent;

public record ItemReadyTodo(TodoListId Id, TodoItemId ItemId) : IDomainEvent;

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());
}

public record TodoItemAdded(TodoItemId ItemId, ItemDescription Description, Temporality Temporality) : IDomainEvent;

public record TodoItemCompleted(TodoListId TodoListId, TodoItemId TodoItemId) : IDomainEvent;

public static class ListExtensions
{
    public static IList<T> Replace<T>(this IList<T> list, T existingElement, T newElement)
    {
        var indexOfExistingElement = list.IndexOf(existingElement);
        if (indexOfExistingElement == -1)
            throw new InvalidOperationException("The existing element does not belong to the list");
        list.Insert(indexOfExistingElement, newElement);
        list.Remove(existingElement);

        return list;
    }

    public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, T existingElement, T newElement)
    {
        foreach (var element in enumerable)
            if (Equals(element, existingElement))
                yield return newElement;
            else
                yield return element;
    }
}