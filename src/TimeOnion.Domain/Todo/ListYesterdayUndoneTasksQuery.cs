using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Domain.Todo;

public record ListYesterdayUndoneTasksQuery : IQuery<IReadOnlyCollection<YesterdayUndoneTodoItem>>;

public record YesterdayUndoneTodoItem(Guid ItemId, string Description);

internal class ListYesterdayUndoneTasksQueryHandler : IQueryHandler<ListYesterdayUndoneTasksQuery,
    IReadOnlyCollection<YesterdayUndoneTodoItem>>
{
    private readonly IReadModelDatabase _database;

    public ListYesterdayUndoneTasksQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<YesterdayUndoneTodoItem>> Handle(ListYesterdayUndoneTasksQuery query)
    {
        var items = await _database.GetAll<TodoListItem>();

        return items
            .Where(x => x.Temporality == Temporality.ThisDay)
            .Where(x => !x.IsDone)
            .Select(x => new YesterdayUndoneTodoItem(x.Id, x.Description))
            .ToArray();
    }
}