using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record DeleteItemAction(TodoListId ListId, TodoItemId ItemId) : IAction<TodoListDetailsState>;

internal class DeleteItemActionHandler : ActionHandlerBase<TodoListDetailsState, DeleteItemAction>
{
    public DeleteItemActionHandler(IStore store, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, DeleteItemAction action)
    {
        var item = state.GetItem(action.ListId, action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            return state.RemoveItem(item);
        }

        await Dispatch(new DeleteTodoItemCommand(action.ListId, action.ItemId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}