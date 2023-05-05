using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class InsertNewItemToDoOnTopOfCategoryActionHandler :
    ActionHandlerBase<TodoListState, TodoListState.InsertNewItemToDoOnTopOfCategory>
{
    public InsertNewItemToDoOnTopOfCategoryActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(
        TodoListState state,
        TodoListState.InsertNewItemToDoOnTopOfCategory action
    )
    {
        await Task.Delay(0);

        return state with
        {
            TodoListDetails = state.TodoListDetails.InsertNewItemOnTopOfCategory(
                action.ListId,
                state.CurrentTimeHorizon,
                action.CategoryId
            )
        };
    }
}