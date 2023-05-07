using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.ActionHandling;

namespace TimeOnion.Pages.TodoListPage.Details.Actions.Items;

internal record RescheduleTodoItemAction(
    TodoListId ListId,
    TodoItemId ItemId,
    TimeHorizons TimeHorizons
) : TodoItemAction(ListId);

internal class RescheduleTodoItemActionHandler
    : ActionApplier<RescheduleTodoItemAction, TodoListDetailsState>
{
    public RescheduleTodoItemActionHandler(
        IStore store,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(store, commandDispatcher, queryDispatcher)
    {
    }

    protected override async Task<TodoListDetailsState> Apply(
        RescheduleTodoItemAction action,
        TodoListDetailsState state
    )
    {
        await Dispatch(new RescheduleTodoItemCommand(
            action.ListId,
            action.ItemId,
            action.TimeHorizons
        ));

        return state with
        {
            TodoListItems = await Dispatch(new ListTodoItemsQuery(action.ListId, state.CurrentTimeHorizon))
        };
    }
}