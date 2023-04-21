using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;

namespace TimeOnion.Pages.TodayTaskPreparation.Actions;

public class DeleteItemActionHandler : ActionHandler<TodoListState.DeleteItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public DeleteItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.DeleteItem action, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new DeleteTodoItemCommand(action.ListId, action.ItemId));

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTimeHorizons));
    }
}