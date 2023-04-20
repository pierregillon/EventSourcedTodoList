using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Pages.TodayTaskPreparation.Steps;

public class FinalizeTodayTasksStep : ITodayTaskPreparationStep
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public FinalizeTodayTasksStep(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    public TodayTaskPreparationSteps Id => TodayTaskPreparationSteps.FinalizeTodayTasks;

    public ITodayTaskPreparationStep? Next() => null;

    public async Task Initialize(TodayTaskPreparationState state) => state.ThisDayUndoneTasks =
        (await _queryDispatcher.Dispatch(new ListUndoneTasksFromTemporalityCommand(Temporality.ThisDay)))
        .Select(x => new SelectableTodoItem(x.ItemId, x.Description, true))
        .ToArray();

    public async Task Save(TodayTaskPreparationState state)
    {
        var itemsToReschedule = state.ThisDayUndoneTasks
            .Where(x => !x.IsSelected)
            .ToArray();

        foreach (var item in itemsToReschedule)
        {
            var command = new RescheduleTodoItemCommand(item.ItemId, Temporality.ThisWeek);
            await _commandDispatcher.Dispatch(command);
        }
    }
}