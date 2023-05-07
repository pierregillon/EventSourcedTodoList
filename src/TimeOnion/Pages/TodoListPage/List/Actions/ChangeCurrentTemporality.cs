using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodoListPage.Details;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.List.Actions;

internal record ChangeCurrentTemporalityAction(TimeHorizons TimeHorizons) : IAction<TodoListState>;

internal class ChangeCurrentTemporalityActionHandler :
    IActionHandler<ChangeCurrentTemporalityAction, TodoListState>
{
    private readonly IStore _store;
    private readonly IQueryDispatcher _queryDispatcher;

    public ChangeCurrentTemporalityActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    )
    {
        _store = store;
        _queryDispatcher = queryDispatcher;
    }


    public async Task Handle(ChangeCurrentTemporalityAction action)
    {
        var detailStates = _store.GetAllStates<TodoListDetailsState>();

        foreach (var state in detailStates)
        {
            var query = new ListTodoItemsQuery(state.TodoListId, action.TimeHorizons);

            var newItems = await _queryDispatcher.Dispatch(query);

            var newState = state with
            {
                CurrentTimeHorizon = action.TimeHorizons,
                TodoListItems = newItems
            };

            _store.UpdateState(state, newState);
        }

        var todoListState = _store.GetState<TodoListState>(Subscriptions.DefaultScope);

        var newTodoListState = todoListState with
        {
            CurrentTimeHorizon = action.TimeHorizons
        };

        _store.SetState(newTodoListState, Subscriptions.DefaultScope);
    }
}