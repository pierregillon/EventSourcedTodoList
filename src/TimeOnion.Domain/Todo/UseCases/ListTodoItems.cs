using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Categories.Core.Events;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;
using TimeOnion.Domain.Todo.Core.Events.Items;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoItemsQuery
    (TodoListId ListId, TimeHorizons TimeHorizon) : IQuery<IReadOnlyCollection<TodoListItemReadModel>>;

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
);

public record TodoListEntry(TodoListId ListId, IReadOnlyCollection<TodoListItemReadModel> Items);

internal class ListTodoItemsQueryHandler :
    IQueryHandler<ListTodoItemsQuery, IReadOnlyCollection<TodoListItemReadModel>>,
    IDomainEventListener<TodoListCreated>,
    IDomainEventListener<TodoListDeleted>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>,
    IDomainEventListener<ItemReadyTodo>,
    IDomainEventListener<TodoItemDescriptionFixed>,
    IDomainEventListener<TodoItemRescheduled>,
    IDomainEventListener<TodoItemDeleted>,
    IDomainEventListener<TodoItemRepositionedAboveAnother>,
    IDomainEventListener<TodoItemRepositionedAtTheEnd>,
    IDomainEventListener<TodoItemCategorized>,
    IDomainEventListener<TodoItemDecategorized>,
    IDomainEventListener<CategoryDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoItemsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<TodoListItemReadModel>> Handle(ListTodoItemsQuery query)
    {
        var list = (await _database.GetAll<TodoListEntry>()).ToArray();

        var todoList = list.Single(x => x.ListId == query.ListId);

        return todoList.Items.Where(item => item.TimeHorizons == query.TimeHorizon).ToArray();
    }

    public async Task On(TodoListCreated domainEvent)
    {
        var readModel =
            new TodoListEntry(domainEvent.ListId, Array.Empty<TodoListItemReadModel>());
        await _database.Add(readModel);
    }

    public async Task On(TodoListDeleted domainEvent) =>
        await _database.Delete<TodoListEntry>(x => x.ListId == domainEvent.ListId);

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItemReadModel(
            domainEvent.ItemId,
            domainEvent.ListId,
            domainEvent.Description.Value,
            false,
            domainEvent.TimeHorizon,
            domainEvent.CategoryId
        );

        if (domainEvent.AboveItemId is not null)
        {
            var aboveItem = (await _database.GetAll<TodoListEntry>())
                .Where(x => x.ListId == domainEvent.ListId)
                .SelectMany(x => x.Items)
                .Single(x => x.Id == domainEvent.AboveItemId);

            await _database.Update<TodoListEntry>(
                x => x.ListId == domainEvent.ListId,
                list =>
                {
                    var items = list.Items.ToList();
                    var aboveItemIndex = items.IndexOf(aboveItem);
                    items.Insert(aboveItemIndex + 1, newItem);
                    return list with { Items = items };
                });
        }
        else
        {
            await _database.Update<TodoListEntry>(
                x => x.ListId == domainEvent.ListId,
                list => list with { Items = list.Items.Append(newItem).ToArray() }
            );
        }
    }

    public async Task On(TodoItemCompleted domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { IsDone = true })
    );

    public async Task On(ItemReadyTodo domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { IsDone = false })
    );

    public async Task On(TodoItemDeleted domainEvent) => await _database.Update<TodoListEntry>(
        x => x.ListId == domainEvent.ListId,
        list => list with { Items = list.Items.Where(x => x.Id != domainEvent.ItemId).ToArray() }
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { Description = domainEvent.NewItemDescription.Value })
    );

    public async Task On(TodoItemRescheduled domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { TimeHorizons = domainEvent.NewTimeHorizon })
    );

    public async Task On(TodoItemRepositionedAboveAnother domainEvent) => await _database.Update<TodoListEntry>(
        x => x.ListId == domainEvent.ListId,
        todoList =>
        {
            var newList = todoList.Items.ToList();
            var item = newList.FirstOrDefault(x => x.Id == domainEvent.ItemId);
            var referenceItem = newList.FirstOrDefault(x => x.Id == domainEvent.ReferenceItemId);
            if (item is null || referenceItem is null)
            {
                return todoList;
            }

            var initialItemIndex = newList.IndexOf(item);
            var referenceItemIndex = newList.IndexOf(referenceItem);
            if (initialItemIndex == referenceItemIndex)
            {
                return todoList;
            }

            newList.Insert(referenceItemIndex, item);

            var oldItemIndex = referenceItemIndex < initialItemIndex
                ? newList.FindLastIndex(x => x == item)
                : newList.FindIndex(x => x == item);

            newList.RemoveAt(oldItemIndex);
            return todoList with { Items = newList };
        }
    );

    public async Task On(TodoItemRepositionedAtTheEnd domainEvent) => await _database.Update<TodoListEntry>(
        x => x.ListId == domainEvent.ListId,
        todoList =>
        {
            var newList = todoList.Items.ToList();
            var item = newList.FirstOrDefault(x => x.Id == domainEvent.ItemId);
            if (item is null)
            {
                return todoList;
            }

            newList.Remove(item);
            newList.Add(item);
            return todoList with { Items = newList };
        }
    );

    public async Task On(TodoItemCategorized domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = domainEvent.NewCategoryId })
    );

    public async Task On(TodoItemDecategorized domainEvent) => await _database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = null })
    );

    public async Task On(CategoryDeleted domainEvent)
    {
        var items = await _database.GetAll<TodoListEntry>();

        var categoryListId = items
            .FirstOrDefault(x => x.Items.Any(i => i.CategoryId == domainEvent.CategoryId))
            ?.ListId;

        if (categoryListId is not null)
        {
            await _database.Update<TodoListEntry>(
                list => list.ListId == categoryListId,
                list => list with
                {
                    Items = list.Items
                        .Select(x => x.CategoryId == domainEvent.CategoryId
                            ? x with { CategoryId = null }
                            : x)
                        .ToArray()
                });
        }
    }

    private static Func<TodoListEntry, TodoListEntry> UpdateItem(
        TodoItemId todoItemId,
        Func<TodoListItemReadModel, TodoListItemReadModel> update
    ) => list =>
    {
        var item = list.Items.FirstOrDefault(x => x.Id == todoItemId);
        if (item is not null)
        {
            return list with { Items = list.Items.Replace(item, update(item)).ToArray() };
        }

        return list;
    };
}