using BlazorState;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

public class InsertNewItemToDoActionHandler : ActionHandler<TodoListState.InsertNewItemToDo>
{
    public InsertNewItemToDoActionHandler(IStore aStore) : base(aStore)
    {
    }

    public override Task Handle(TodoListState.InsertNewItemToDo aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        if (aAction.ItemId is null)
        {
            state.TodoListDetails.InsertAtTheEnd(aAction.ListId, state.CurrentTimeHorizon);
        }
        else
        {
            var item = state.TodoListDetails.GetItem(aAction.ListId, aAction.ItemId);

            state.TodoListDetails.InsertNewItemTodoAfter(item);
        }

        return Task.CompletedTask;
    }
}