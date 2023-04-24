using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core.Events;
using TimeOnion.Domain.Todo.Core.Events.Items;

namespace TimeOnion.Domain.Todo.Core;

public class TodoList : EventSourcedAggregate<TodoListId>
{
    private readonly List<TodoListItem> _items = new();
    private TodoListName _name = default!;

    private TodoList(TodoListId id) : base(id)
    {
    }

    public static TodoList New(TodoListName name)
    {
        var todoListId = TodoListId.New();
        var todoList = new TodoList(todoListId);
        todoList.StoreEvent(new TodoListCreated(todoListId, name));
        return todoList;
    }

    public static TodoList Rehydrate(TodoListId id, IReadOnlyCollection<IDomainEvent> eventHistory)
    {
        var todoList = new TodoList(id);

        todoList.LoadFromHistory(eventHistory);

        return todoList;
    }

    public void Rename(TodoListName newName)
    {
        if (_name != newName)
        {
            StoreEvent(new TodoListRenamed(Id, _name, newName));
        }
    }

    public void Delete() => StoreEvent(new TodoListDeleted(Id));

    public void AddItem(TodoItemDescription description, TimeHorizons timeHorizons) =>
        StoreEvent(new TodoItemAdded(Id, TodoItemId.New(), description, timeHorizons));

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

    public void FixItemDescription(TodoItemId itemId, TodoItemDescription newItemDescription)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot fix item description: unknown item");
        }

        StoreEvent(new TodoItemDescriptionFixed(Id, item.Id, item.Description, newItemDescription));
    }

    public void Reschedule(TodoItemId itemId, TimeHorizons anotherTimeHorizons)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot reschedule item to do: unknown item");
        }

        if (item.TimeHorizons != anotherTimeHorizons)
        {
            StoreEvent(new TodoItemRescheduled(Id, item.Id, item.TimeHorizons, anotherTimeHorizons));
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

    public void RepositionItemAboveAnother(TodoItemId itemId, TodoItemId referenceItemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot reposition the item: unknown item");
        }

        var referenceItem = _items.FirstOrDefault(x => x.Id == referenceItemId);

        if (referenceItem is null)
        {
            throw new InvalidOperationException("Cannot reposition the item: unknown reference item");
        }

        StoreEvent(new TodoItemRepositionedAboveAnother(Id, item.Id, referenceItemId));
    }

    public void RepositionItemAtTheEnd(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot reposition the item at the end: unknown item");
        }

        StoreEvent(new TodoItemRepositionedAtTheEnd(Id, item.Id));
    }

    public void CategorizeItem(TodoItemId itemId, Category category)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot categorize: unknown item");
        }

        if (item.CategoryId != category.Id)
        {
            StoreEvent(new TodoItemCategorized(Id, item.Id, item.CategoryId, category.Id));
        }
    }

    public void DecategorizeItem(TodoItemId itemId)
    {
        var item = _items.FirstOrDefault(x => x.Id == itemId);

        if (item is null)
        {
            throw new InvalidOperationException("Cannot decategorize: unknown item");
        }

        if (item.CategoryId is not null)
        {
            StoreEvent(new TodoItemDecategorized(Id, item.Id));
        }
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
                _items.Add(new TodoListItem(added.ItemId, added.Description, added.TimeHorizon, CategoryId.None));
                break;

            case TodoItemDescriptionFixed descriptionFixed:
                item = _items.Single(x => x.Id == descriptionFixed.ItemId);
                _items.Replace(item, item with { Description = descriptionFixed.NewItemDescription });
                break;

            case TodoItemRescheduled rescheduled:
                item = _items.Single(x => x.Id == rescheduled.ItemId);
                _items.Replace(item, item with { TimeHorizons = rescheduled.NewTimeHorizon });
                break;

            case TodoItemCategorized categorized:
                item = _items.Single(x => x.Id == categorized.ItemId);
                _items.Replace(item, item with { CategoryId = categorized.NewCategoryId });
                break;

            case TodoItemDecategorized decategorized:
                item = _items.Single(x => x.Id == decategorized.ItemId);
                _items.Replace(item, item with { CategoryId = null });
                break;

            case TodoItemDeleted deleted:
                item = _items.Single(x => x.Id == deleted.ItemId);
                _items.Remove(item);
                break;
        }
    }

    private record TodoListItem(
        TodoItemId Id,
        TodoItemDescription Description,
        TimeHorizons TimeHorizons,
        CategoryId? CategoryId
    );
}