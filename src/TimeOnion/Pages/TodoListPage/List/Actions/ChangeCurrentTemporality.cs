using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record ChangeCurrentTemporalityAction(TimeHorizons TimeHorizons) : IAction<TodoListState>;

internal class ChangeCurrentTemporalityActionApplier :
    ISyncActionApplier<ChangeCurrentTemporalityAction, TodoListState>
{
    public ChangeCurrentTemporalityActionApplier(IStore store) => Store = store;

    public IStore Store { get; }

    public TodoListState Apply(ChangeCurrentTemporalityAction action, TodoListState state) => state with
    {
        CurrentTimeHorizon = action.TimeHorizons
    };
}