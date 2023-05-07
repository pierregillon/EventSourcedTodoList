using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record InsertNewItemToDoAtTheEndActionAction(TodoListId ListId) : IAction<TodoListDetailsState>;

internal class InsertNewItemToDoAtTheEndActionHandler :
    ActionHandlerBase<TodoListDetailsState, InsertNewItemToDoAtTheEndActionAction>
{
    public InsertNewItemToDoAtTheEndActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        InsertNewItemToDoAtTheEndActionAction actionAction
    )
    {
        await Task.Delay(0);

        return state.InsertAtTheEnd(actionAction.ListId, state.CurrentTimeHorizon);
    }
}