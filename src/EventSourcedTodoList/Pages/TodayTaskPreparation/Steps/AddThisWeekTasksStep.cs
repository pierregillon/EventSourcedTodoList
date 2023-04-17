using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Pages.TodayTaskPreparation.Steps;

public class AddThisWeekTasksStep : ITodayTaskPreparationStep
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public AddThisWeekTasksStep(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    public TodayTaskPreparationSteps Id => TodayTaskPreparationSteps.AddThisWeekTasks;

    public ITodayTaskPreparationStep? Next() => new FinalizeTodayTasksStep(_queryDispatcher, _commandDispatcher);

    public async Task Initialize(TodayTaskPreparationState state) => state.ThisWeekUndoneTasks =
        (await _queryDispatcher.Dispatch(new ListUndoneTasksFromTemporalityCommand(Temporality.ThisWeek)))
        .Select(item => new SelectableTodoItem(item.ItemId, item.Description, false))
        .ToArray();

    public async Task Save(TodayTaskPreparationState state)
    {
        var itemToTake = state.ThisWeekUndoneTasks
            .Where(x => x.IsSelected)
            .ToArray();

        foreach (var item in itemToTake)
        {
            var command = new RescheduleTodoItemCommand(new TodoItemId(item.ItemId), Temporality.ThisDay);
            await _commandDispatcher.Dispatch(command);
        }
    }
}