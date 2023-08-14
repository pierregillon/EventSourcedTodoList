using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Projections;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoListsQuery : IQuery<IReadOnlyCollection<TodoListReadModel>>;

public record TodoListReadModel(TodoListId Id, string Name);

internal record ListTodoListsQueryHandler(
    IUserScopedReadModelDatabase Database
) : IQueryHandler<ListTodoListsQuery, IReadOnlyCollection<TodoListReadModel>>
{
    public async Task<IReadOnlyCollection<TodoListReadModel>> Handle(ListTodoListsQuery query) =>
        (await Database.GetAll<TodoListProjectItem>())
        .Select(x=> new TodoListReadModel(x.Id, x.Name))
        .ToArray();
}

