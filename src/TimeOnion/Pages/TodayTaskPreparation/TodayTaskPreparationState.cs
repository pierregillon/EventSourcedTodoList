using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.UseCases;
using TimeOnion.Pages.TodayTaskPreparation.Steps;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.TodayTaskPreparation;

public record TodayTaskPreparationState(
    ITodayTaskPreparationStep? CurrentStep,
    IEnumerable<SelectableTodoItem> YesterdayUndoneTasks,
    IEnumerable<SelectableTodoItem> ThisWeekUndoneTasks,
    IEnumerable<SelectableTodoItem> ThisDayUndoneTasks
) : IState
{
    public static TodayTaskPreparationState Initialize() => new(
        null,
        new List<SelectableTodoItem>(),
        new List<SelectableTodoItem>(),
        new List<SelectableTodoItem>()
    );

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