using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record InsertNewItemToDoOnTopOfCategoryAction(
    TodoListId ListId,
    CategoryId CategoryId
) : TodoItemAction(ListId);

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

        var newItem = new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            action.ListId,
            string.Empty,
            false,
            state.CurrentTimeHorizon,
            action.CategoryId
        );

        return state with
        {
            TodoListItems = state.TodoListItems.Prepend(newItem).ToList()
        };
    }
}