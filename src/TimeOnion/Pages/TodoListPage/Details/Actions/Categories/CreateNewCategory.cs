using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record CreateNewCategoryAction(TodoListId ListId) : TodoItemAction(ListId);

internal class CreateNewCategoryActionHandler : ActionHandlerBase<TodoListDetailsState, CreateNewCategoryAction>
{
    public CreateNewCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        TodoListDetailsState state,
        CreateNewCategoryAction action
    )
    {
        await Dispatch(new CreateNewCategoryCommand(new CategoryName("Nouvelle Catégorie"), action.ListId));

        return state with
        {
            Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
        };
    }
}