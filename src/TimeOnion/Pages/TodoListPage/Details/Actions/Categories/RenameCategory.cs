using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record RenameCategoryAction(
    CategoryId Id,
    string Name,
    TodoListId ListId
) : TodoItemAction(ListId);

internal class RenameCategoryActionHandler : ActionApplier<RenameCategoryAction, TodoListDetailsState>
{
    public RenameCategoryActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(RenameCategoryAction action, TodoListDetailsState state)
    {
        await Dispatch(new RenameCategoryCommand(action.Id, new CategoryName(action.Name)));

        return state with
        {
            Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
        };
    }
}