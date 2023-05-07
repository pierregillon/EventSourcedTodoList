using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.List;

internal record ChangeCurrentTemporalityAction(TimeHorizons TimeHorizons) : IAction<TodoListDetailsState>;

internal class ChangeCurrentTemporalityActionHandler :
    ActionHandlerBase<TodoListDetailsState, ChangeCurrentTemporalityAction>
{
    private readonly IStore _store;

    public ChangeCurrentTemporalityActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
        _store = store;
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        ChangeCurrentTemporalityAction action
    )
    {
        var todoListState = _store.GetState<TodoListState>();
        
        state = state with
        {
            CurrentTimeHorizon = action.TimeHorizons
        };

        foreach (var todoList in todoListState.TodoLists)
        {
            var items = await Dispatch(new ListTodoItemsQuery(todoList.Id, state.CurrentTimeHorizon));

            state = state.UpdateItems(todoList.Id, items);
        }

        return state;
    }
}