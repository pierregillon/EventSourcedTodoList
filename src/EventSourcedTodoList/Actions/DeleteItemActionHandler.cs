using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class DeleteItemActionHandler : ActionHandler<TodoListState.DeleteItem>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public DeleteItemActionHandler(IStore aStore, ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.DeleteItem action, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new DeleteTodoItemCommand(new TodoItemId(action.ItemId)));

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery(state.CurrentTemporality));
    }
}