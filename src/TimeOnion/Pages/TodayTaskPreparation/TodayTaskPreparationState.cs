using BlazorState;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Pages.TodayTaskPreparation.Steps;

namespace TimeOnion.Pages.TodayTaskPreparation;

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

public record SelectableTodoItem(TodoListId ListId, TodoItemId ItemId, string Description, bool IsSelected)
{
    public bool IsSelected { get; set; } = IsSelected;

    public static SelectableTodoItem From(YesterdayUndoneTodoItem item) =>
        new(item.ListId, item.ItemId, item.Description, false);

    public static SelectableTodoItem From(ThisWeekUndoneTodoItem item) =>
        new(item.ListId, item.ItemId, item.Description, false);
}