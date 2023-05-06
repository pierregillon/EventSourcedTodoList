using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

internal record CreateNewCategoryAction(TodoListId ListId) : IAction<TodoListState>;

internal class CreateNewCategoryActionHandler : ActionHandlerBase<TodoListState, CreateNewCategoryAction>
{
    public CreateNewCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, CreateNewCategoryAction action)
    {
        await Dispatch(new CreateNewCategoryCommand(new CategoryName("Nouvelle Catégorie"), action.ListId));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateCategories(action.ListId,
                categories)
        };
    }
}