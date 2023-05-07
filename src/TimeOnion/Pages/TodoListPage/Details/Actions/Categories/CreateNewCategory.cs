using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record CreateNewCategoryAction(TodoListId ListId) : TodoItemAction(ListId);

internal class CreateNewCategoryActionHandler : ActionApplier<CreateNewCategoryAction, TodoListDetailsState>
{
    public CreateNewCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        CreateNewCategoryAction action,
        TodoListDetailsState state
    )
    {
        await Dispatch(new CreateNewCategoryCommand(new CategoryName("Nouvelle Catégorie"), action.ListId));

        return state with
        {
            Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
        };
    }
}