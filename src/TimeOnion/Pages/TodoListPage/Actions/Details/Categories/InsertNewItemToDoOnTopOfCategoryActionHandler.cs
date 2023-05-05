using BlazorState;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class InsertNewItemToDoOnTopOfCategoryActionHandler : ActionHandler<TodoListState.InsertNewItemToDoOnTopOfCategory>
{
    public InsertNewItemToDoOnTopOfCategoryActionHandler(IStore aStore) : base(aStore)
    {
    }

    public override Task Handle(TodoListState.InsertNewItemToDoOnTopOfCategory aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.TodoListDetails.InsertNewItemOnTopOfCategory(aAction.ListId, state.CurrentTimeHorizon, aAction.CategoryId);

        return Task.CompletedTask;
    }
}