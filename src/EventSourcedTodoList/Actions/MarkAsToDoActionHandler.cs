using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;
using MediatR;

namespace EventSourcedTodoList.Actions;

public class MarkAsToDoActionHandler : ActionHandler<TodoListState.MarkItemAsToDo>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public MarkAsToDoActionHandler(IStore aStore, ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task<Unit> Handle(TodoListState.MarkItemAsToDo action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        await _commandDispatcher.Dispatch(new MarkItemAsToDoCommand(new TodoItemId(action.ItemId)));

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery());

        return Unit.Value;
    }
}