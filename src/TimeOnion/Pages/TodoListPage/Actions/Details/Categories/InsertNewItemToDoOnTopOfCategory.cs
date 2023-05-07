using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record InsertNewItemToDoOnTopOfCategoryAction(
    TodoListId ListId,
    CategoryId CategoryId
) : IAction<TodoListDetailsState>;

internal class InsertNewItemToDoOnTopOfCategoryActionHandler :
    ActionHandlerBase<TodoListDetailsState, InsertNewItemToDoOnTopOfCategoryAction>
{
    public InsertNewItemToDoOnTopOfCategoryActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        InsertNewItemToDoOnTopOfCategoryAction action
    )
    {
        await Task.Delay(0);

        return state.InsertNewItemOnTopOfCategory(
            action.ListId,
            state.CurrentTimeHorizon,
            action.CategoryId
        );
    }
}