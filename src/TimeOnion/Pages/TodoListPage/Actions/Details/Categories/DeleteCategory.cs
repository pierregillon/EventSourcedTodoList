using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record DeleteCategoryAction(
    CategoryId Id,
    TodoListId ListId
) : IAction<TodoListDetailsState>;

internal class DeleteCategoryActionHandler : ActionHandlerBase<TodoListDetailsState, DeleteCategoryAction>
{
    public DeleteCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, DeleteCategoryAction action)
    {
        await Dispatch(new DeleteCategoryCommand(action.Id));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state
            .UpdateCategories(action.ListId, categories)
            .UpdateItems(action.ListId, items);
    }
}