using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.List;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private readonly List<TodoListItem> _items = new();
    private TodoListName _name = default!;

    private TodoList(TodoListId id, IEnumerable<IDomainEvent> eventHistory) : base(id)
    {
        foreach (var domainEvent in eventHistory)
        {
            Apply(domainEvent);
        }
    }

    public static TodoList New(TodoListName name)
    {
        var todoListId = TodoListId.New();
        var todoList = new TodoList(todoListId, Enumerable.Empty<IDomainEvent>());
        todoList.StoreEvent(new TodoListCreated(todoListId, name));
        return todoList;
    }

    public static TodoList Rehydrate(TodoListId id, IEnumerable<IDomainEvent> eventHistory) => new(id, eventHistory);

    public void AddItem(ItemDescription description, Temporality temporality) =>
        StoreEvent(new TodoItemAdded(Id, TodoItemId.New(), description, temporality));

    public void MarkItemAsDone(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot complete the item: unknown item");
        }

        StoreEvent(new TodoItemCompleted(Id, itemId));
    }

    public void MarkItemAsToDo(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot mark the item as to do: unknown item");
        }

        StoreEvent(new ItemReadyTodo(Id, item.Id));
    }

    public void FixItemDescription(TodoItemId itemId, ItemDescription newItemDescription)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot fix item description: unknown item");
        }

        StoreEvent(new TodoItemDescriptionFixed(Id, item.Id, item.Description, newItemDescription));
    }

    public void Reschedule(TodoItemId itemId, Temporality anotherTemporality)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot reschedule item to do: unknown item");
        }

        if (item.Temporality != anotherTemporality)
        {
            StoreEvent(new TodoItemRescheduled(Id, item.Id, item.Temporality, anotherTemporality));
        }
    }

    public void Delete(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot delete the item: unknown item");
        }

        StoreEvent(new TodoItemDeleted(Id, itemId));
    }

    protected sealed override void Apply(IDomainEvent domainEvent)
    {
        TodoListItem item;

        switch (domainEvent)
        {
            case TodoListCreated created:
                _name = created.Name;
                break;

            case TodoListRenamed renamed:
                _name = renamed.NewName;
                break;

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

            case TodoItemDeleted deleted:
                item = _items.Single(x => x.Id == deleted.ItemId);
                _items.Remove(item);
                break;
        }
    }

    private record TodoListItem(TodoItemId Id, ItemDescription Description, Temporality Temporality);

    public void Rename(TodoListName newName)
    {
        if (_name != newName)
        {
            StoreEvent(new TodoListRenamed(Id, _name, newName));
        }
    }
}

public record TodoListRenamed(TodoListId Id, TodoListName PreviousName, TodoListName NewName) : TodoListDomainEvent(Id);

public record TodoListName(string Value)
{
    public string Value { get; } = string.IsNullOrWhiteSpace(Value)
        ? throw new ArgumentException("A todo list name cannot be null or empty")
        : Value;
}

public record TodoListCreated(TodoListId Id, TodoListName Name) : TodoListDomainEvent(Id);

public record TodoItemDeleted(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);

public record TodoItemRescheduled(
    TodoListId Id,
    TodoItemId ItemId,
    Temporality PreviousTemporality,
    Temporality NewTemporality
) : TodoListDomainEvent(Id);

public enum Temporality
{
    ThisDay,
    ThisWeek,
    ThisMonth,
    ThisQuarter,
    ThisYear,
    ThisLife
}

public static class TemporalityExtensions
{
    public static string ToText(this Temporality temporality) => temporality switch
    {
        Temporality.ThisDay => "Ce jour",
        Temporality.ThisWeek => "Cette semaine",
        Temporality.ThisMonth => "Ce mois",
        Temporality.ThisQuarter => "Ce trimestre",
        Temporality.ThisYear => "Cette année",
        Temporality.ThisLife => "Cette vie",
        _ => throw new ArgumentOutOfRangeException(nameof(temporality), temporality, null)
    };

    public static Temporality Next(this Temporality temporality)
    {
        var next = temporality + 1;
        var temporalities = Enum.GetValues<Temporality>();
        return temporalities.Contains(next) ? next : temporalities.Last();
    }

    public static Temporality Previous(this Temporality temporality)
    {
        var next = temporality - 1;
        var temporalities = Enum.GetValues<Temporality>();
        return temporalities.Contains(next) ? next : temporalities.First();
    }
}

public record TodoItemDescriptionFixed(
    TodoListId Id,
    TodoItemId ItemId,
    ItemDescription PreviousItemDescription,
    ItemDescription NewItemDescription
) : TodoListDomainEvent(Id);

public record ItemReadyTodo(TodoListId Id, TodoItemId ItemId) : TodoListDomainEvent(Id);

public record TodoItemId(Guid Value)
{
    public static TodoItemId New() => new(Guid.NewGuid());
}

public record TodoItemAdded(
    TodoListId TodoListId,
    TodoItemId ItemId,
    ItemDescription Description,
    Temporality Temporality
) : TodoListDomainEvent(TodoListId);

public record TodoListDomainEvent(TodoListId TodoListId) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => TodoListId.Value;
}

public record TodoItemCompleted(TodoListId TodoListId, TodoItemId TodoItemId) : TodoListDomainEvent(TodoListId);

public static class ListExtensions
{
    public static IList<T> Replace<T>(this IList<T> list, T existingElement, T newElement)
    {
        var indexOfExistingElement = list.IndexOf(existingElement);
        if (indexOfExistingElement == -1)
        {
            throw new InvalidOperationException("The existing element does not belong to the list");
        }

        list.Insert(indexOfExistingElement, newElement);
        list.Remove(existingElement);

        return list;
    }

    public static IEnumerable<T> Replace<T>(this IEnumerable<T> enumerable, T existingElement, T newElement)
    {
        foreach (var element in enumerable)
        {
            if (Equals(element, existingElement))
            {
                yield return newElement;
            }
            else
            {
                yield return element;
            }
        }
    }
}