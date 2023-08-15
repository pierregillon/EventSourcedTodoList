using Microsoft.Extensions.Logging;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core.Events;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;
using TimeOnion.Domain.Todo.Core.Events.Items;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Todo.Projections;

internal record TodoListItemsProjection(IReadModelDatabase Database, ILogger<TodoListItemsProjection> Logger) :
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
    public async Task On(TodoListCreated domainEvent)
    {
        var readModel = new TodoListEntry(
            domainEvent.ListId,
            Array.Empty<TodoListItemReadModel>(),
            domainEvent.As<IUserDomainEvent>().UserId
        );
        await Database.Add(readModel);
    }

    public async Task On(TodoListDeleted domainEvent) =>
        await Database.Delete<TodoListEntry>(x => x.ListId == domainEvent.ListId);

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItemReadModel(
            domainEvent.ItemId,
            domainEvent.ListId,
            domainEvent.Description.Value,
            null,
            domainEvent.TimeHorizon,
            domainEvent.CategoryId
        );
        
        var aboveItem = await GetAboveItem(domainEvent);

        if (aboveItem is null)
        {
            await Database.Update<TodoListEntry>(
                x => x.ListId == domainEvent.ListId,
                list => list with { Items = list.Items.Append(newItem).ToArray() }
            );
        }
        else
        {
            await Database.Update<TodoListEntry>(
                x => x.ListId == domainEvent.ListId,
                list =>
                {
                    var items = list.Items.ToList();
                    var aboveItemIndex = items.IndexOf(aboveItem);
                    items.Insert(aboveItemIndex + 1, newItem);
                    return list with { Items = items };
                });
        }
    }

    private async Task<TodoListItemReadModel?> GetAboveItem(TodoItemAdded domainEvent)
    {
        if (domainEvent.AboveItemId is null)
        {
            return null;
        }

        var aboveItem = (await Database.GetAll<TodoListEntry>())
            .Where(x => x.ListId == domainEvent.ListId)
            .SelectMany(x => x.Items)
            .SingleOrDefault(x => x.Id == domainEvent.AboveItemId);

        if (aboveItem is null)
        {
            Logger.LogWarning(
                "Cannot find above item {0}: adding the item {1} to the end.",
                domainEvent.AboveItemId, 
                domainEvent.ItemId
            );
        }

        return aboveItem;
    }

    public async Task On(TodoItemCompleted domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { DoneDate = ((IDomainEvent)domainEvent).CreatedAt })
    );

    public async Task On(ItemReadyTodo domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { DoneDate = null })
    );

    public async Task On(TodoItemDeleted domainEvent) => await Database.Update<TodoListEntry>(
        x => x.ListId == domainEvent.ListId,
        list => list with { Items = list.Items.Where(x => x.Id != domainEvent.ItemId).ToArray() }
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { Description = domainEvent.NewItemDescription.Value })
    );

    public async Task On(TodoItemRescheduled domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { TimeHorizons = domainEvent.NewTimeHorizon })
    );

    public async Task On(TodoItemRepositionedAboveAnother domainEvent) => await Database.Update<TodoListEntry>(
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

    public async Task On(TodoItemRepositionedAtTheEnd domainEvent) => await Database.Update<TodoListEntry>(
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

    public async Task On(TodoItemCategorized domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = domainEvent.NewCategoryId })
    );

    public async Task On(TodoItemDecategorized domainEvent) => await Database.Update(
        x => x.ListId == domainEvent.ListId,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = null })
    );

    public async Task On(CategoryDeleted domainEvent)
    {
        var items = await Database.GetAll<TodoListEntry>();

        var categoryListId = items
            .FirstOrDefault(x => x.Items.Any(i => i.CategoryId == domainEvent.CategoryId))
            ?.ListId;

        if (categoryListId is not null)
        {
            await Database.Update<TodoListEntry>(
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



public record TodoListEntry(
    TodoListId ListId, 
    IReadOnlyCollection<TodoListItemReadModel> Items,
    UserId UserId
) : IUserScopedProjection;