using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Projections;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoItemsQuery(TodoListId ListId, TimeHorizons TimeHorizon)
    : IQuery<IReadOnlyCollection<TodoListItemReadModel>>;

public record ListTodoItems(IUserScopedReadModelDatabase Database, IClock Clock)
    : IQueryHandler<ListTodoItemsQuery, IReadOnlyCollection<TodoListItemReadModel>>
{
    public async Task<IReadOnlyCollection<TodoListItemReadModel>> Handle(ListTodoItemsQuery query)
    {
        var list = (await Database.GetAll<TodoListEntry>()).ToArray();

        var todoList = list.Single(x => x.ListId == query.ListId);

        return todoList.Items
            .Where(item => item.TimeHorizons == query.TimeHorizon)
            .Where(item => !item.IsDone || !item.TimeHorizonStillRunning(Clock.Now()))
            .ToArray();
    }
}

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    DateTime? DoneDate,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
)
{
    public bool IsDone => DoneDate.HasValue;

    public bool TimeHorizonStillRunning(DateTime now) =>
        DoneDate.HasValue && now >= TimeHorizons.GetEndDate(DoneDate.Value);
}