using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record InsertNewItemToDoOnTopOfCategoryAction
    (TodoListId ListId, CategoryId CategoryId) : IAction<TodoListState>;

internal class InsertNewItemToDoOnTopOfCategoryActionHandler :
    ActionHandlerBase<TodoListState, InsertNewItemToDoOnTopOfCategoryAction>
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
        InsertNewItemToDoOnTopOfCategoryAction action
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