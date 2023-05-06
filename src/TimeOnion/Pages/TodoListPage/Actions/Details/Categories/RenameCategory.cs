using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record RenameCategoryAction(
    CategoryId Id,
    string Name,
    TodoListId ListId
) : IAction<TodoListState>;

internal class RenameCategoryActionHandler : ActionHandlerBase<TodoListState, RenameCategoryAction>
{
    public RenameCategoryActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, RenameCategoryAction action)
    {
        await Dispatch(new RenameCategoryCommand(action.Id, new CategoryName(action.Name)));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateCategories(action.ListId, categories)
        };
    }
}