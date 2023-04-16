using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Pages.TodayTaskPreparation.Steps;

public class EndYesterdayTasksStep : ITodayTaskPreparationStep
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IQueryDispatcher _queryDispatcher;

    public EndYesterdayTasksStep(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
        _queryDispatcher = queryDispatcher;
    }

    public TodayTaskPreparationSteps Id => TodayTaskPreparationSteps.EndYesterdayTasks;

    public ITodayTaskPreparationStep? Next() => new AddThisWeekTasksStep(_queryDispatcher, _commandDispatcher);

    public async Task Initialize(TodayTaskPreparationState state) => state.YesterdayUndoneTasks =
        (await _queryDispatcher.Dispatch(new ListYesterdayUndoneTasksQuery()))
        .Select(SelectableTodoItem.From)
        .ToArray();

    public async Task Save(TodayTaskPreparationState state)
    {
        var itemToMarkAsDone = state.YesterdayUndoneTasks
            .Where(x => x.IsSelected)
            .ToArray();

        foreach (var item in itemToMarkAsDone)
        {
            await _commandDispatcher.Dispatch(new MarkItemAsDoneCommand(new TodoItemId(item.ItemId)));
        }
    }
}