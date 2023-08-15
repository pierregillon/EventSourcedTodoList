using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record DeleteItemAction(TodoListId ListId, TodoItemId ItemId) : TodoItemAction(ListId);

internal record DeleteItemActionHandler(
    IStore Store,
    ICommandDispatcher CommandDispatcher,
    IQueryDispatcher QueryDispatcher,
    IActionEventPublisher ActionEventPublisher
) : IActionApplier<DeleteItemAction, TodoListDetailsState>
{
    public async Task<TodoListDetailsState> Apply(DeleteItemAction action, TodoListDetailsState state)
    {
        var item = state.TodoListItems.Single(x => x.Id == action.ItemId);

        if (item is TodoListItemReadModelBeingCreated)
        {
            var newItems = state.TodoListItems.Except(new[] { item }).ToList();

            await ActionEventPublisher.Publish<TodoItemDeleted, TodoListDetailsState>(
                new TodoItemDeleted(action.ItemId)
            );

            return state with
            {
                TodoListItems = newItems
            };
        }

        await CommandDispatcher.Dispatch(new DeleteTodoItemCommand(action.ListId, action.ItemId));

        await ActionEventPublisher.Publish<TodoItemDeleted, TodoListDetailsState>(
            new TodoItemDeleted(action.ItemId)
        );

        return state with
        {
            TodoListItems =
                await QueryDispatcher.Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}

public record TodoItemDeleted(TodoItemId ItemId) : IActionEvent<TodoListDetailsState>, IState;