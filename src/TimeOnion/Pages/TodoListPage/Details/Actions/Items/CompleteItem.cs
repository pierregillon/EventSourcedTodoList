using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record MarkItemAsDoneAction(
    TodoListId ListId,
    TodoItemId ItemId
) : TodoItemAction(ListId);

internal class CompleteItemActionHandler : ActionApplier<MarkItemAsDoneAction, TodoListDetailsState>
{
    public CompleteItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(MarkItemAsDoneAction action, TodoListDetailsState state)
    {
        await Dispatch(new MarkItemAsDoneCommand(action.ListId, action.ItemId));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}