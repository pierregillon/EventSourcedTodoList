using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Projections;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListUndoneTasksFromTemporalityQuery
    (TimeHorizons TimeHorizons) : IQuery<IReadOnlyCollection<ThisWeekUndoneTodoItem>>;

public record ThisWeekUndoneTodoItem(TodoListId ListId, TodoItemId ItemId, string Description);

internal class ListUndoneTasksFromTemporalityQueryHandler : IQueryHandler<ListUndoneTasksFromTemporalityQuery,
    IReadOnlyCollection<ThisWeekUndoneTodoItem>>
{
    private readonly IUserScopedReadModelDatabase _database;

    public ListUndoneTasksFromTemporalityQueryHandler(IUserScopedReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<ThisWeekUndoneTodoItem>> Handle(ListUndoneTasksFromTemporalityQuery query)
    {
        var items = await _database.GetAll<TodoListEntry>();

        return items
            .SelectMany(x => x.Items)
            .Where(x => x.TimeHorizon == query.TimeHorizons)
            .Where(x => !x.IsDone)
            .Select(x => new ThisWeekUndoneTodoItem(x.ListId, x.Id, x.Description))
            .ToArray();
    }
}