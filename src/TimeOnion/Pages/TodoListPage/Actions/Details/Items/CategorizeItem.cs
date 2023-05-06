using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Domain.Todo.UseCases.Categorization;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record CategorizeItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    CategoryId CategoryId
) : IAction<TodoListState>;

internal class CategorizeItemActionHandler : ActionHandlerBase<TodoListState, CategorizeItemAction>
{
    public CategorizeItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, CategorizeItemAction action)
    {
        await Dispatch(new CategorizeTodoItemCommand(
            action.ListId,
            action.ItemId,
            action.CategoryId
        ));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}