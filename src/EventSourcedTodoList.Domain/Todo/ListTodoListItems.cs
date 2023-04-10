using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record ListTodoListItemsQuery : IQuery<IReadOnlyCollection<TodoListItem>>;

internal class ListTodoListItemsQueryHandler : IQueryHandler<ListTodoListItemsQuery, IReadOnlyCollection<TodoListItem>>,
    IDomainEventListener<TodoItemAdded>,
    IDomainEventListener<TodoItemCompleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListItemsQueryHandler(IReadModelDatabase database)
    {
        _database = database;
    }

    public async Task On(TodoItemAdded domainEvent)
    {
        await _database.Add(new TodoListItem(domainEvent.ItemId.Value, domainEvent.Description.Value, false));
    }

    public async Task On(TodoItemCompleted domainEvent)
    {
        var items = await _database.GetAll<TodoListItem>();

        var item = items.First(x => x.Id == domainEvent.TodoItemId.Value);

        item.MarkAsCompleted();
    }

    public async Task<IReadOnlyCollection<TodoListItem>> Handle(ListTodoListItemsQuery query)
    {
        var items = await _database.GetAll<TodoListItem>();
        return items.ToArray();
    }
}