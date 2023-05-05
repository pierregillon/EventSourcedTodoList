using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class InsertNewItemToDoAtTheEndActionHandler :
    ActionHandlerBase<TodoListState, TodoListState.InsertNewItemToDoAtTheEnd>
{
    public InsertNewItemToDoAtTheEndActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        TodoListState.InsertNewItemToDoAtTheEnd action
    )
    {
        await Task.Delay(0);

        return state with
        {
            TodoListDetails = state.TodoListDetails.InsertAtTheEnd(action.ListId, state.CurrentTimeHorizon)
        };
    }
}