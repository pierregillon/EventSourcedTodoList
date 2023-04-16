using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Pages.TodayTaskPreparation.Steps;

namespace EventSourcedTodoList.Pages.TodayTaskPreparation.Actions;

public class LoadTodayTaskPreparationActionHandler : ActionHandler<TodayTaskPreparationState.Load>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public LoadTodayTaskPreparationActionHandler(IStore aStore, IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher) : base(aStore)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    public override async Task Handle(TodayTaskPreparationState.Load aAction, CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodayTaskPreparationState>();

        state.CurrentStep = new EndYesterdayTasksStep(_queryDispatcher, _commandDispatcher);

        state.YesterdayUndoneTasks = (await _queryDispatcher.Dispatch(new ListYesterdayUndoneTasksQuery()))
            .Select(SelectableTodoItem.From)
            .ToArray();
    }
}