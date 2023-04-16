using BlazorState;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class MoveToNextPreparationStepActionHandler : ActionHandler<TodayTaskPreparationState.MoveToNextPreparationStep>
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public MoveToNextPreparationStepActionHandler(IStore aStore, ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher) : base(aStore)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public override async Task Handle(TodayTaskPreparationState.MoveToNextPreparationStep aAction,
        CancellationToken aCancellationToken)
    {
        var state = Store.GetState<TodayTaskPreparationState>();

        await Save(state);

        state.CurrentStep++;

        switch (state.CurrentStep)
        {
            case TodayTaskPreparationState.TodayTaskPreparationSteps.EndYesterdayTasks:
                state.YesterdayUndoneTasks = (await _queryDispatcher.Dispatch(new ListYesterdayUndoneTasksQuery()))
                    .Select(SelectableTodoItem.From)
                    .ToArray();
                break;
            
            case TodayTaskPreparationState.TodayTaskPreparationSteps.AddThisWeekTasks:
                state.ThisWeekUndoneTasks =
                    (await _queryDispatcher.Dispatch(new ListUndoneTasksFromTemporalityCommand(Temporality.ThisWeek)))
                    .Select(item => new SelectableTodoItem(item.ItemId, item.Description, false))
                    .ToArray();
                break;
            
            case TodayTaskPreparationState.TodayTaskPreparationSteps.RemoveNotWanted:
                state.ThisDayUndoneTasks =
                    (await _queryDispatcher.Dispatch(new ListUndoneTasksFromTemporalityCommand(Temporality.ThisDay)))
                    .Select(x => new SelectableTodoItem(x.ItemId, x.Description, true))
                    .ToArray();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private async Task Save(TodayTaskPreparationState state)
    {
        switch (state.CurrentStep)
        {
            case TodayTaskPreparationState.TodayTaskPreparationSteps.EndYesterdayTasks:
                var itemToMarkAsDone = state.YesterdayUndoneTasks
                    .Where(x => x.IsSelected)
                    .ToArray();

                foreach (var item in itemToMarkAsDone)
                {
                    await _commandDispatcher.Dispatch(new MarkItemAsDoneCommand(new TodoItemId(item.ItemId)));
                }
                break;
            
            case TodayTaskPreparationState.TodayTaskPreparationSteps.AddThisWeekTasks:
                var itemToTake = state.ThisWeekUndoneTasks
                    .Where(x => x.IsSelected)
                    .ToArray();

                foreach (var item in itemToTake)
                {
                    var command = new RescheduleTodoItemCommand(new TodoItemId(item.ItemId), Temporality.ThisDay);
                    await _commandDispatcher.Dispatch(command);
                }
                break;
            
            case TodayTaskPreparationState.TodayTaskPreparationSteps.RemoveNotWanted:
                var itemsToReschedule = state.ThisDayUndoneTasks
                    .Where(x => !x.IsSelected)
                    .ToArray();

                foreach (var item in itemsToReschedule)
                {
                    var command = new RescheduleTodoItemCommand(new TodoItemId(item.ItemId), Temporality.ThisWeek);
                    await _commandDispatcher.Dispatch(command);
                }
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}