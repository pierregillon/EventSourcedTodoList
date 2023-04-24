using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;

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
        (await _queryDispatcher.Dispatch(new ListUndoneTasksFromTemporalityQuery(TimeHorizons.ThisDay)))
        .Select(x => new SelectableTodoItem(x.ListId, x.ItemId, x.Description, true))
        .ToArray();

    public async Task Save(TodayTaskPreparationState state)
    {
        var itemsToReschedule = state.ThisDayUndoneTasks
            .Where(x => !x.IsSelected)
            .ToArray();

        foreach (var item in itemsToReschedule)
        {
            var command = new RescheduleTodoItemCommand(item.ListId, item.ItemId, TimeHorizons.ThisWeek);
            await _commandDispatcher.Dispatch(command);
        }
    }
}