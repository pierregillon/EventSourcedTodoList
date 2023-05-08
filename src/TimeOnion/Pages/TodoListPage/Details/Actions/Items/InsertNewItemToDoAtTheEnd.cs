using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record InsertNewItemToDoAtTheEndAction(TodoListId ListId) : TodoItemAction(ListId);

internal class InsertNewItemToDoAtTheEndActionHandler :
    ActionApplier<InsertNewItemToDoAtTheEndAction, TodoListDetailsState>
{
    public InsertNewItemToDoAtTheEndActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        InsertNewItemToDoAtTheEndAction action,
        TodoListDetailsState state
    )
    {
        await Task.Delay(0);

        var newItem = new TodoListItemReadModelBeingCreated(
            TodoItemId.New(),
            action.ListId,
            string.Empty,
            null,
            state.CurrentTimeHorizon,
            CategoryId.None
        );

        return state with
        {
            TodoListItems = state.TodoListItems.Append(newItem).ToList()
        };
    }
}