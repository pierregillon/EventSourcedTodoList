using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record DeleteItemAction(TodoListId ListId, TodoItemId ItemId) : IAction<TodoListState>;

internal class DeleteItemActionHandler : ActionHandlerBase<TodoListState, DeleteItemAction>
{
    public DeleteItemActionHandler(IStore store, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, DeleteItemAction action)
    {
        var item = state.TodoListDetails.GetItem(action.ListId, action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            return state with
            {
                TodoListDetails = state.TodoListDetails.RemoveItem(item)
            };
        }

        await Dispatch(new DeleteTodoItemCommand(action.ListId, action.ItemId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}