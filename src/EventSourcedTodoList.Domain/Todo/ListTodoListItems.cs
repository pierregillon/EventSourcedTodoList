using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record ListTodoListItemsQuery : IQuery<IReadOnlyCollection<TodoListItem>>;

internal class ListTodoListItemsQueryHandler : IQueryHandler<ListTodoListItemsQuery, IReadOnlyCollection<TodoListItem>>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>,
    IDomainEventListener<ItemReadyTodo>,
    IDomainEventListener<TodoItemDescriptionFixed>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListItemsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task On(ItemReadyTodo domainEvent)
    {
        var items = await _database.GetAll<TodoListItem>();

        var item = items.First(x => x.Id == domainEvent.ItemId.Value);

        item.MarkAsToDo();
    }

    public async Task On(TodoItemAdded domainEvent) =>
        await _database.Add(new TodoListItem(domainEvent.ItemId.Value, domainEvent.Description.Value, false,
            domainEvent.Temporality));

    public async Task On(TodoItemCompleted domainEvent)
    {
        var items = await _database.GetAll<TodoListItem>();

        var item = items.First(x => x.Id == domainEvent.TodoItemId.Value);

        item.MarkAsDone();
    }

    public async Task On(TodoItemDescriptionFixed domainEvent)
    {
        var items = await _database.GetAll<TodoListItem>();

        var item = items.First(x => x.Id == domainEvent.ItemId.Value);

        item.UpdateDescription(domainEvent.NewItemDescription);
    }

    public async Task<IReadOnlyCollection<TodoListItem>> Handle(ListTodoListItemsQuery query) =>
        (await _database.GetAll<TodoListItem>()).ToArray();
}