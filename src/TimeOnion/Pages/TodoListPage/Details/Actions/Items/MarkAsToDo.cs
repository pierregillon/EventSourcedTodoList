using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record MarkItemAsToDoAction(TodoListId ListId, TodoItemId ItemId) : TodoItemAction(ListId);

internal class MarkItemAsToDoActionHandler : ActionApplier<MarkItemAsToDoAction, TodoListDetailsState>
{
    public MarkItemAsToDoActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    )
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(MarkItemAsToDoAction action, TodoListDetailsState state)
    {
        await Dispatch(new MarkItemAsToDoCommand(action.ListId, action.ItemId));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}