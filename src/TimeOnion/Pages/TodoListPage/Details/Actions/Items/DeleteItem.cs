using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record DeleteItemAction(TodoListId ListId, TodoItemId ItemId) : TodoItemAction(ListId);

internal class DeleteItemActionHandler : ActionHandlerBase<TodoListDetailsState, DeleteItemAction>
{
    public DeleteItemActionHandler(IStore store, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, DeleteItemAction action)
    {
        var item = state.TodoListItems.Single(x => x.Id == action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            return state with
            {
                TodoListItems = state.TodoListItems.Except(new[] { item }).ToList()
            };
        }

        await Dispatch(new DeleteTodoItemCommand(action.ListId, action.ItemId));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}