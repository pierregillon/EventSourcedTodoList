using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage;

public record TodoListState(
    TimeHorizons CurrentTimeHorizon,
    IEnumerable<TodoListReadModel> TodoLists,
    TodoListDetails TodoListDetails
) : IState
{
    // ReSharper disable once UnusedMember.Global
    public static TodoListState Initialize() =>
        new(
            TimeHorizons.ThisDay,
            new List<TodoListReadModel>(),
            TodoListDetails.Empty
        );
}