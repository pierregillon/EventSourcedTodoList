using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record InsertNewItemToDoAtTheEndActionAction(TodoListId ListId) : IAction<TodoListState>;

internal class InsertNewItemToDoAtTheEndActionHandler :
    ActionHandlerBase<TodoListState, InsertNewItemToDoAtTheEndActionAction>
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
        InsertNewItemToDoAtTheEndActionAction actionAction
    )
    {
        await Task.Delay(0);

        return state with
        {
            TodoListDetails = state.TodoListDetails.InsertAtTheEnd(actionAction.ListId, state.CurrentTimeHorizon)
        };
    }
}