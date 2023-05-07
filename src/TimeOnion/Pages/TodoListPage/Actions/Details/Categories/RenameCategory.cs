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
) : IAction<TodoListDetailsState>;

internal class RenameCategoryActionHandler : ActionHandlerBase<TodoListDetailsState, RenameCategoryAction>
{
    public RenameCategoryActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, RenameCategoryAction action)
    {
        await Dispatch(new RenameCategoryCommand(action.Id, new CategoryName(action.Name)));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state.UpdateCategories(action.ListId, categories);
    }
}