using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record DeleteCategoryAction(
    CategoryId Id,
    TodoListId ListId
) : TodoItemAction(ListId);

internal class DeleteCategoryActionHandler : ActionApplier<DeleteCategoryAction, TodoListDetailsState>
{
    public DeleteCategoryActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(DeleteCategoryAction action, TodoListDetailsState state)
    {
        await Dispatch(new DeleteCategoryCommand(action.Id));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon)),
            Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
        };
    }
}