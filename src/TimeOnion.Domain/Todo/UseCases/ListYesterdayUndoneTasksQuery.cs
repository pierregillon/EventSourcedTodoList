using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListYesterdayUndoneTasksQuery : IQuery<IReadOnlyCollection<YesterdayUndoneTodoItem>>;

public record YesterdayUndoneTodoItem(TodoListId ListId, TodoItemId ItemId, string Description);

internal class ListYesterdayUndoneTasksQueryHandler : IQueryHandler<ListYesterdayUndoneTasksQuery,
    IReadOnlyCollection<YesterdayUndoneTodoItem>>
{
    private readonly IReadModelDatabase _database;

    public ListYesterdayUndoneTasksQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<YesterdayUndoneTodoItem>> Handle(ListYesterdayUndoneTasksQuery query)
    {
        var items = await _database.GetAll<TodoListReadModel>();

        return items
            .SelectMany(x => x.Items)
            .Where(x => x.TimeHorizons == TimeHorizons.ThisDay)
            .Where(x => !x.IsDone)
            .Select(x => new YesterdayUndoneTodoItem(x.ListId, x.Id, x.Description))
            .ToArray();
    }
}