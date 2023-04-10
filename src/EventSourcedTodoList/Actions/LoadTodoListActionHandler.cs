using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using MediatR;

namespace EventSourcedTodoList.Actions;

public class LoadTodoListActionHandler : ActionHandler<TodoListState.LoadTodoList>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadTodoListActionHandler(IStore aStore,
        IQueryDispatcher queryDispatcher) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadTodoList action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery());

        return Unit.Value;
    }
}