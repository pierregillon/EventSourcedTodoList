using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Categories;

internal record DeleteCategoryAction(
    CategoryId Id,
    TodoListId ListId
) : TodoItemAction(ListId);

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

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon)),
            Categories = await Dispatch(new ListCategoriesQuery(action.ListId))
        };
    }
}