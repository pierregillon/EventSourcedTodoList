using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record LoadCategoriesAction(
    TodoListId ListId
) : IAction<TodoListState>;

internal class LoadCategoriesActionHandler : ActionHandlerBase<TodoListState, LoadCategoriesAction>
{
    public LoadCategoriesActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, LoadCategoriesAction action)
    {
        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateCategories(action.ListId, categories)
        };
    }
}