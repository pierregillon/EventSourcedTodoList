using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListUndoneTasksFromTemporalityQuery
    (TimeHorizons TimeHorizons) : IQuery<IReadOnlyCollection<ThisWeekUndoneTodoItem>>;

public record ThisWeekUndoneTodoItem(TodoListId ListId, TodoItemId ItemId, string Description);

internal class ListUndoneTasksFromTemporalityQueryHandler : IQueryHandler<ListUndoneTasksFromTemporalityQuery,
    IReadOnlyCollection<ThisWeekUndoneTodoItem>>
{
    private readonly IReadModelDatabase _database;

    public ListUndoneTasksFromTemporalityQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<ThisWeekUndoneTodoItem>> Handle(ListUndoneTasksFromTemporalityQuery query)
    {
        var items = await _database.GetAll<TodoListReadModel>();

        return items
            .SelectMany(x => x.Items)
            .Where(x => x.TimeHorizons == query.TimeHorizons)
            .Where(x => !x.IsDone)
            .Select(x => new ThisWeekUndoneTodoItem(x.ListId, x.Id, x.Description))
            .ToArray();
    }
}