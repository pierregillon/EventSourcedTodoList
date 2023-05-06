using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Categories;

public class CreateNewCategoryActionHandler : ActionHandlerBase<TodoListState, TodoListState.CreateNewCategory>
{
    public CreateNewCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, TodoListState.CreateNewCategory action)
    {
        await Dispatch(new CreateNewCategory(new CategoryName("Nouvelle Catégorie"), action.ListId));

        var categories = await Dispatch(new ListCategoriesQuery(action.ListId));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateCategories(action.ListId,
                categories)
        };
    }
}