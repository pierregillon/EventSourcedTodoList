using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage;

public record TodoListState(
    IEnumerable<TodoListReadModel> TodoLists
) : IState
{
    // ReSharper disable once UnusedMember.Global
    public static TodoListState Initialize() =>
        new(
            new List<TodoListReadModel>()
        );
}