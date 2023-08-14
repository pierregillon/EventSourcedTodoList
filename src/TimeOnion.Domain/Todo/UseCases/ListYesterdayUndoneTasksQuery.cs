using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Projections;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListYesterdayUndoneTasksQuery : IQuery<IReadOnlyCollection<YesterdayUndoneTodoItem>>;

public record YesterdayUndoneTodoItem(TodoListId ListId, TodoItemId ItemId, string Description);

internal class ListYesterdayUndoneTasksQueryHandler : IQueryHandler<ListYesterdayUndoneTasksQuery,
    IReadOnlyCollection<YesterdayUndoneTodoItem>>
{
    private readonly IUserScopedReadModelDatabase _database;

    public ListYesterdayUndoneTasksQueryHandler(IUserScopedReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<YesterdayUndoneTodoItem>> Handle(ListYesterdayUndoneTasksQuery query)
    {
        var items = await _database.GetAll<TodoListEntry>();

        return items
            .SelectMany(x => x.Items)
            .Where(x => x.TimeHorizons == TimeHorizons.ThisDay)
            .Where(x => !x.IsDone)
            .Select(x => new YesterdayUndoneTodoItem(x.ListId, x.Id, x.Description))
            .ToArray();
    }
}