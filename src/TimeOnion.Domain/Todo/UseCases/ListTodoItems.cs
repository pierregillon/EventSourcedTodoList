using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Projections;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoItemsQuery(TodoListId ListId, TimeHorizons TimeHorizon) 
    : IQuery<IReadOnlyCollection<TodoListItemReadModel>>;

public record ListTodoItems(IUserScopedReadModelDatabase Database, IClock Clock) 
    : IQueryHandler<ListTodoItemsQuery, IReadOnlyCollection<TodoListItemReadModel>> {

    public async Task<IReadOnlyCollection<TodoListItemReadModel>> Handle(ListTodoItemsQuery query)
    {
        var list = (await Database.GetAll<TodoListEntry>()).ToArray();

        var todoList = list.Single(x => x.ListId == query.ListId);

        return todoList.Items
            .Where(item => item.TimeHorizons == query.TimeHorizon)
            .Where(item => !item.IsDoneForLong(Clock.Now()))
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

    public bool IsDoneForLong(DateTime now) =>
        DoneDate.HasValue && now >= DoneDate.Value.Add(TimeHorizons.ToTimeSpan());
}