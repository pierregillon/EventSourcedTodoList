using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class FixItemDescriptionActionHandler : ActionHandler<TodoListState.FixItemDescription>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public FixItemDescriptionActionHandler(IStore aStore, ICommandDispatcher commandDispatcher,
        IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodoListState.FixItemDescription action, CancellationToken token)
    {
        var state = Store.GetState<TodoListState>();

        var command =
            new FixItemDescriptionCommand(
                new TodoItemId(action.ItemId),
                new ItemDescription(action.NewDescription)
            );

        await _commandDispatcher.Dispatch(command);

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery());
    }
}