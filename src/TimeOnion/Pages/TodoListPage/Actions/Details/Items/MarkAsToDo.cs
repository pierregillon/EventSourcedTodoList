using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record MarkItemAsToDoAction(TodoListId ListId, TodoItemId ItemId) : IAction<TodoListDetailsState>;

internal class MarkItemAsToDoActionHandler : ActionHandlerBase<TodoListDetailsState, MarkItemAsToDoAction>
{
    public MarkItemAsToDoActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    )
        : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(TodoListDetailsState state, MarkItemAsToDoAction action)
    {
        await Dispatch(new MarkItemAsToDoCommand(action.ListId, action.ItemId));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state.UpdateItems(action.ListId, items);
    }
}