using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;
using TimeOnion.Domain.Todo.Core.Events.Items;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoListsQuery(TimeHorizons TimeHorizons) : IQuery<IReadOnlyCollection<TodoListReadModel>>;

public record TodoListReadModel(TodoListId Id, string Name, IReadOnlyCollection<TodoListItemReadModel> Items);

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
);

internal class ListTodoListsQueryHandler : IQueryHandler<ListTodoListsQuery, IReadOnlyCollection<TodoListReadModel>>,
    IDomainEventListener<TodoListCreated>,
    IDomainEventListener<TodoListRenamed>,
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
    IDomainEventListener<TodoItemDecategorized>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task On(TodoListCreated domainEvent)
    {
        var readModel =
            new TodoListReadModel(domainEvent.Id, domainEvent.Name.Value, Array.Empty<TodoListItemReadModel>());
        await _database.Add(readModel);
    }

    public async Task On(TodoListRenamed domainEvent) => await _database.Update<TodoListReadModel>(
        x => x.Id == domainEvent.Id,
        x => x with { Name = domainEvent.NewName.Value }
    );

    public async Task On(TodoListDeleted domainEvent) =>
        await _database.Delete<TodoListReadModel>(x => x.Id == domainEvent.Id);

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItemReadModel(
            domainEvent.ItemId,
            domainEvent.Id,
            domainEvent.Description.Value,
            false,
            domainEvent.TimeHorizon,
            CategoryId.None
        );

        await _database.Update<TodoListReadModel>(
            x => x.Id == domainEvent.Id,
            list => list with { Items = list.Items.Append(newItem).ToArray() }
        );
    }

    public async Task On(TodoItemCompleted domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { IsDone = true })
    );

    public async Task On(ItemReadyTodo domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { IsDone = false })
    );

    public async Task On(TodoItemDeleted domainEvent) => await _database.Update<TodoListReadModel>(
        x => x.Id == domainEvent.Id,
        list => list with { Items = list.Items.Where(x => x.Id != domainEvent.ItemId).ToArray() }
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { Description = domainEvent.NewItemDescription.Value })
    );

    public async Task On(TodoItemRescheduled domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { TimeHorizons = domainEvent.NewTimeHorizon })
    );

    public async Task On(TodoItemRepositionedAboveAnother domainEvent) => await _database.Update<TodoListReadModel>(
        x => x.Id == domainEvent.Id,
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

    public async Task On(TodoItemRepositionedAtTheEnd domainEvent) => await _database.Update<TodoListReadModel>(
        x => x.Id == domainEvent.Id,
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
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = domainEvent.NewCategoryId })
    );

    public async Task On(TodoItemDecategorized domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { CategoryId = null })
    );

    public async Task<IReadOnlyCollection<TodoListReadModel>> Handle(ListTodoListsQuery query)
    {
        var list = (await _database.GetAll<TodoListReadModel>()).ToArray();

        return list.Select(x => x with
        {
            Items = x.Items
                .Where(item => item.TimeHorizons == query.TimeHorizons).ToArray()
        }).ToArray();
    }

    private static Func<TodoListReadModel, TodoListReadModel> UpdateItem(
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