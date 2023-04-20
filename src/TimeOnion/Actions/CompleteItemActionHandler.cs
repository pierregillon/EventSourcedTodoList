using BlazorState;
using MediatR;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;

namespace TimeOnion.Actions;

public class CompleteItemActionHandler : ActionHandler<TodoListState.MarkItemAsDone>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CompleteItemActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task<Unit> Handle(TodoListState.MarkItemAsDone action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new MarkItemAsDoneCommand(action.ListId, action.ItemId));

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTemporality));

        return Unit.Value;
    }
}