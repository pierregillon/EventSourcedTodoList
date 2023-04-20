using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Actions;

public class FixItemDescriptionActionHandler : ActionHandler<TodoListState.FixItemDescription>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public FixItemDescriptionActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.FixItemDescription action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new FixItemDescriptionCommand(
                action.ItemId,
                new ItemDescription(action.NewDescription)
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTemporality));
    }
}