using BlazorState;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class InsertNewItemToDoAtTheEndActionHandler : ActionHandler<TodoListState.InsertNewItemToDoAtTheEnd>
{
    public InsertNewItemToDoAtTheEndActionHandler(IStore aStore) : base(aStore)
    {
    }

    public override Task Handle(TodoListState.InsertNewItemToDoAtTheEnd aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoListDetails.InsertAtTheEnd(aAction.ListId, state.CurrentTimeHorizon);

        return Task.CompletedTask;
    }
}