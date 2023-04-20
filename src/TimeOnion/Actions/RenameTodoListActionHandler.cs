using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Actions;

public class RenameTodoListActionHandler : ActionHandler<TodoListState.RenameTodoList>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RenameTodoListActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.RenameTodoList aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RenameTodoListCommand(
                aAction.ListId,
                new TodoListName(aAction.NewName)
            );

        await _commandDispatcher.Dispatch(command);

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTemporality));
    }
}