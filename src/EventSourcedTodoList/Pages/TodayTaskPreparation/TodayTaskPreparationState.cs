using BlazorState;
using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Pages.TodayTaskPreparation.Steps;

namespace EventSourcedTodoList.Pages.TodayTaskPreparation;

public class TodayTaskPreparationState : State<TodayTaskPreparationState>
{
    public ITodayTaskPreparationStep? CurrentStep { get; set; }

    public IReadOnlyCollection<SelectableTodoItem> YesterdayUndoneTasks { get; set; } =
        Array.Empty<SelectableTodoItem>();

    public IEnumerable<SelectableTodoItem> ThisWeekUndoneTasks { get; set; } =
        Array.Empty<SelectableTodoItem>();

    public IEnumerable<SelectableTodoItem> ThisDayUndoneTasks { get; set; } =
        Array.Empty<SelectableTodoItem>();

    public override void Initialize()
    {
    }

    public record Load : IAction;

    public record MoveToNextPreparationStep : IAction;
}

public record SelectableTodoItem(Guid ItemId, string Description, bool IsSelected)
{
    public bool IsSelected { get; set; } = IsSelected;

    public static SelectableTodoItem From(YesterdayUndoneTodoItem item) =>
        new(item.ItemId, item.Description, false);

    public static SelectableTodoItem From(ThisWeekUndoneTodoItem item) =>
        new(item.ItemId, item.Description, false);
}