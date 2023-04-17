using BlazorState;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;

namespace TimeOnion.Actions;

public class ChangeCurrentTemporalityActionHandler : ActionHandler<TodoListState.ChangeCurrentTemporality>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public ChangeCurrentTemporalityActionHandler(IStore aStore, IQueryDispatcher queryDispatcher) : base(aStore) =>
        _queryDispatcher = queryDispatcher;

    public override async Task Handle(TodoListState.ChangeCurrentTemporality action,
        CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodoListState>();

        state.CurrentTemporality = action.Temporality;
        state.Items = await _queryDispatcher.Dispatch(new ListTodoListItemsQuery(action.Temporality));
    }
}