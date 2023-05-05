using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class DeleteCategoryActionHandler : ActionHandlerBase<TodoListState, TodoListState.DeleteCategory>
{
    public DeleteCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, TodoListState.DeleteCategory action)
    {
        await Dispatch(new DeleteCategoryCommand(action.Id));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails
                .UpdateCategories(action.ListId, categories)
                .UpdateItems(action.ListId, items)
        };
    }
}