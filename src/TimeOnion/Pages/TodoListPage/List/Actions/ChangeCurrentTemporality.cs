using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record ChangeCurrentTemporalityAction(TimeHorizons TimeHorizons) : IAction<TodoListState>;

internal class ChangeCurrentTemporalityActionHandler :
    SynchronousStateActionHandler<TodoListState, ChangeCurrentTemporalityAction>
{
    public ChangeCurrentTemporalityActionHandler(IStore store) : base(store)
    {
    }

    protected override TodoListState ApplySynchronously(TodoListState state, ChangeCurrentTemporalityAction action) =>
        state with
        {
            CurrentTimeHorizon = action.TimeHorizons
        };
}