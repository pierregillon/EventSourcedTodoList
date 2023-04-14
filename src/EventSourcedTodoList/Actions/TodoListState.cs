using BlazorState;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public IReadOnlyCollection<TodoListItem> Items { get; set; } = Array.Empty<TodoListItem>();
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public Temporality CurrentTemporality { get; set; } = Temporality.ThisDay;

    public override void Initialize()
    {
    }

    public record LoadTodoList(Temporality Temporality) : IAction;

    public record AddNewItem(string? Text) : IAction;

    public record MarkItemAsDone(Guid ItemId) : IAction;

    public record MarkItemAsToDo(Guid ItemId) : IAction;

    public record FixItemDescription(Guid ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(Guid ItemId, Temporality Temporality) : IAction;

    public record ChangeCurrentTemporality(Temporality Temporality) : IAction;

    public record DeleteItem(Guid ItemId) : IAction;
}