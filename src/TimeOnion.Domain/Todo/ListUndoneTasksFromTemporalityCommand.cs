using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListUndoneTasksFromTemporalityCommand
    (Temporality Temporality) : IQuery<IReadOnlyCollection<ThisWeekUndoneTodoItem>>;

public record ThisWeekUndoneTodoItem(TodoItemId ItemId, string Description);

internal class ListUndoneTasksFromTemporalityCommandHandler : IQueryHandler<ListUndoneTasksFromTemporalityCommand,
    IReadOnlyCollection<ThisWeekUndoneTodoItem>>
{
    private readonly IReadModelDatabase _database;

    public ListUndoneTasksFromTemporalityCommandHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<ThisWeekUndoneTodoItem>> Handle(ListUndoneTasksFromTemporalityCommand query)
    {
        var items = await _database.GetAll<TodoListItemReadModel>();

        return items
            .Where(x => x.Temporality == query.Temporality)
            .Where(x => !x.IsDone)
            .Select(x => new ThisWeekUndoneTodoItem(x.Id, x.Description))
            .ToArray();
    }
}