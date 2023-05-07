using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record CategorizeItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    CategoryId CategoryId
) : TodoItemAction(ListId);

internal class CategorizeItemActionHandler : ActionApplier<CategorizeItemAction, TodoListDetailsState>
{
    public CategorizeItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(CategorizeItemAction action, TodoListDetailsState state)
    {
        await Dispatch(new CategorizeTodoItemCommand(
            action.ListId,
            action.ItemId,
            action.CategoryId
        ));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}