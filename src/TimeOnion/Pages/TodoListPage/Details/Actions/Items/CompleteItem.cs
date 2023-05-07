using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record MarkItemAsDoneAction(
    TodoListId ListId,
    TodoItemId ItemId
) : TodoItemAction(ListId);

internal class CompleteItemActionHandler : ActionHandlerBase<TodoListDetailsState, MarkItemAsDoneAction>
{
    public CompleteItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, MarkItemAsDoneAction action)
    {
        await Dispatch(new MarkItemAsDoneCommand(action.ListId, action.ItemId));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}