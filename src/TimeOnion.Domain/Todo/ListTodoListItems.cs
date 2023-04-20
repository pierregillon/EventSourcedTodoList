using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListTodoListsQuery(Temporality Temporality) : IQuery<IReadOnlyCollection<TodoListReadModel>>;

internal class ListTodoListsQueryHandler : IQueryHandler<ListTodoListsQuery, IReadOnlyCollection<TodoListReadModel>>,
    IDomainEventListener<TodoListCreated>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>,
    IDomainEventListener<ItemReadyTodo>,
    IDomainEventListener<TodoItemDescriptionFixed>,
    IDomainEventListener<TodoItemRescheduled>,
    IDomainEventListener<TodoItemDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task On(ItemReadyTodo domainEvent) => await _database.Update(
        x => x.Id == domainEvent.TodoListId,
        UpdateItem(domainEvent.ItemId, item => item with { IsDone = false })
    );

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItemReadModel(
            domainEvent.ItemId,
            domainEvent.TodoListId,
            domainEvent.Description.Value,
            false,
            domainEvent.Temporality,
            false
        );

        await _database.Update<TodoListReadModel>(
            x => x.Id == domainEvent.TodoListId,
            list => list with { Items = list.Items.Append(newItem).ToArray() }
        );
    }

    public async Task On(TodoItemCompleted domainEvent) => await _database.Update(
        x => x.Id == domainEvent.TodoListId,
        UpdateItem(domainEvent.TodoItemId, item => item with { IsDone = true })
    );

    public async Task On(TodoItemDeleted domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { IsDeleted = true })
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { Description = domainEvent.NewItemDescription.Value })
    );

    public async Task On(TodoItemRescheduled domainEvent) => await _database.Update(
        x => x.Id == domainEvent.Id,
        UpdateItem(domainEvent.ItemId, item => item with { Temporality = domainEvent.NewTemporality })
    );

    public async Task<IReadOnlyCollection<TodoListReadModel>> Handle(ListTodoListsQuery query)
    {
        var list = (await _database.GetAll<TodoListReadModel>()).ToArray();

        return list.Select(x => x with
        {
            Items = x.Items
                .Where(item => !item.IsDeleted)
                .Where(item => item.Temporality == query.Temporality).ToArray()
        }).ToArray();
    }

    public async Task On(TodoListCreated domainEvent)
    {
        var readModel =
            new TodoListReadModel(domainEvent.Id, domainEvent.Name.Value, Array.Empty<TodoListItemReadModel>());
        await _database.Add(readModel);
    }


    private static Func<TodoListReadModel, TodoListReadModel> UpdateItem(
        TodoItemId todoItemId,
        Func<TodoListItemReadModel, TodoListItemReadModel> update
    ) => list =>
    {
        var item = list.Items.FirstOrDefault(x => x.Id == todoItemId);
        if (item is null)
        {
            throw new InvalidOperationException($"Unable to find item with id {todoItemId.Value}");
        }

        return list with { Items = list.Items.Replace(item, update(item)).ToArray() };
    };
}