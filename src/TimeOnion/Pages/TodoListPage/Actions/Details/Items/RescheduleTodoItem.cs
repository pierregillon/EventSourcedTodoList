using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodoListPage.Actions.Details.Items;

internal record RescheduleTodoItemAction
    (TodoListId ListId, TodoItemId ItemId, TimeHorizons TimeHorizons) : IAction<TodoListState>;

internal class RescheduleTodoItemActionHandler
    : ActionHandlerBase<TodoListState, RescheduleTodoItemAction>
{
    public RescheduleTodoItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListState> Apply(TodoListState state, RescheduleTodoItemAction action)
    {
        await Dispatch(new RescheduleTodoItemCommand(
            action.ListId,
            action.ItemId,
            action.TimeHorizons
        ));

        var items = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon));

        return state with
        {
            TodoListDetails = state.TodoListDetails.UpdateItems(action.ListId, items)
        };
    }
}