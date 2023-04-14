using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class RescheduleTodoItemActionHandler : ActionHandler<TodoListState.RescheduleTodoItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public RescheduleTodoItemActionHandler(IStore aStore, ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.RescheduleTodoItem action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new RescheduleTodoItemCommand(
                new TodoItemId(action.ItemId),
                action.Temporality
            );

        await _commandDispatcher.Dispatch(command);

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery(state.CurrentTemporality));
    }
}