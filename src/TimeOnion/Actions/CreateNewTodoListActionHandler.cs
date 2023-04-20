using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Actions;

public class CreateNewTodoListActionHandler : ActionHandler<TodoListState.CreateNewTodoList>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public CreateNewTodoListActionHandler(
        IStore aStore,
        ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher
    ) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.CreateNewTodoList aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new CreateNewTodoListCommand(new TodoListName("Nouvelle todo liste")));

        state.TodoLists = await _queryDispatcher.Dispatch(new ListTodoListsQuery(state.CurrentTemporality));
    }
}