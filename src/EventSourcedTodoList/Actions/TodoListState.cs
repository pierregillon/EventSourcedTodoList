using BlazorState;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public Dictionary<Temporality, IEnumerable<TodoListItem>> Items { get; set; } = new();
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public Temporality CurrentTemporality { get; set; } = Temporality.ThisDay;

    public override void Initialize()
    {
    }

    public record LoadTodoList(Temporality Temporality) : IAction;

    public record AddNewItem(string? Text, Temporality Temporality) : IAction;

    public record MarkItemAsDone(Guid ItemId) : IAction;

    public record MarkItemAsToDo(Guid ItemId) : IAction;

    public record FixItemDescription(Guid ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(Guid ItemId, Temporality Temporality) : IAction;

    public record ChangeCurrentTemporality(Temporality Temporality) : IAction;
}