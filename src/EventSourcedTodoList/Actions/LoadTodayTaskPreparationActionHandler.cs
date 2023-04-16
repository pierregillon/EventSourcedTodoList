using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;

namespace EventSourcedTodoList.Actions;

public class LoadTodayTaskPreparationActionHandler : ActionHandler<TodayTaskPreparationState.Load>
{
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadTodayTaskPreparationActionHandler(IStore aStore, IQueryDispatcher queryDispatcher) : base(aStore) =>
        _queryDispatcher = queryDispatcher;

    public override async Task Handle(TodayTaskPreparationState.Load aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodayTaskPreparationState>();

        state.CurrentStep = TodayTaskPreparationState.TodayTaskPreparationSteps.EndYesterdayTasks;

        state.YesterdayUndoneTasks = (await _queryDispatcher.Dispatch(new ListYesterdayUndoneTasksQuery()))
            .Select(SelectableTodoItem.From)
            .ToArray();
    }
}