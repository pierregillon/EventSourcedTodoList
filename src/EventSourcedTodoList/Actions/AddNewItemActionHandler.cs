using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using MediatR;

namespace EventSourcedTodoList.Actions;

public class AddNewItemActionHandler : ActionHandler<TodoListState.AddNewItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AddNewItemActionHandler(IStore aStore, ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task<Unit> Handle(TodoListState.AddNewItem action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new AddItemToDoCommand(action.Text));

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery());

        return Unit.Value;
    }
}