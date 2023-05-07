using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record LoadCategoriesAction(
    TodoListId ListId
) : IAction<TodoListDetailsState>;

internal class LoadCategoriesActionHandler : ActionHandlerBase<TodoListDetailsState, LoadCategoriesAction>
{
    public LoadCategoriesActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, LoadCategoriesAction action)
    {
        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state.UpdateCategories(action.ListId, categories);
    }
}