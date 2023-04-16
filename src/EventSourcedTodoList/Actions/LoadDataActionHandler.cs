using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using MediatR;

namespace EventSourcedTodoList.Actions;

public class LoadDataActionHandler : ActionHandler<TodoListState.LoadData>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadDataActionHandler(IStore aStore,
        IQueryDispatcher queryDispatcher) : base(aStore) => _queryDispatcher = queryDispatcher;

    public override async Task<Unit> Handle(TodoListState.LoadData action, CancellationToken cancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery(state.CurrentTemporality));

        return Unit.Value;
    }
}