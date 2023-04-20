using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;

namespace TimeOnion.Actions;

public class RescheduleTodoItemActionHandler : ActionHandler<TodoListState.RescheduleTodoItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RescheduleTodoItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.RescheduleTodoItem action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RescheduleTodoItemCommand(
                action.ListId,
                action.ItemId,
                action.Temporality
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTemporality));
    }
}