using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListTodoListsQuery(Temporality Temporality) : IQuery<IReadOnlyCollection<TodoListReadModel>>;

internal class ListTodoListsQueryHandler : IQueryHandler<ListTodoListsQuery, IReadOnlyCollection<TodoListReadModel>>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>,
    IDomainEventListener<ItemReadyTodo>,
    IDomainEventListener<TodoItemDescriptionFixed>,
    IDomainEventListener<TodoItemRescheduled>,
    IDomainEventListener<TodoItemDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task On(ItemReadyTodo domainEvent) => await _database.Update<TodoListItemReadModel>(
        x => x.Id == domainEvent.ItemId,
        item => item with { IsDone = false }
    );

    public async Task On(TodoItemAdded domainEvent)
    {
        var newItem = new TodoListItemReadModel(
            domainEvent.ItemId,
            domainEvent.Description.Value,
            false,
            domainEvent.Temporality,
            false
        );
        await _database.Add(newItem);
    }

    public async Task On(TodoItemCompleted domainEvent) => await _database.Update<TodoListItemReadModel>(
        x => x.Id == domainEvent.TodoItemId,
        item => item with { IsDone = true }
    );

    public async Task On(TodoItemDeleted domainEvent) => await _database.Update<TodoListItemReadModel>(
        x => x.Id == domainEvent.ItemId,
        item => item with { IsDeleted = true }
    );

    public async Task On(TodoItemDescriptionFixed domainEvent) => await _database.Update<TodoListItemReadModel>(
        x => x.Id == domainEvent.ItemId,
        item => item with { Description = domainEvent.NewItemDescription.Value }
    );

    public async Task On(TodoItemRescheduled domainEvent) => await _database.Update<TodoListItemReadModel>(
        x => x.Id == domainEvent.ItemId,
        item => item with { Temporality = domainEvent.NewTemporality }
    );

    public async Task<IReadOnlyCollection<TodoListReadModel>> Handle(ListTodoListsQuery query)
    {
        var items = (await _database.GetAll<TodoListItemReadModel>())
            .Where(x => x.IsDeleted == false)
            .Where(x => x.Temporality == query.Temporality)
            .ToArray();

        return new[]
        {
            new TodoListReadModel(TodoListId.Unique, "Ma todo liste", items)
        };
    }
}