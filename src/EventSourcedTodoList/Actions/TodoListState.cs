using BlazorState;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public IReadOnlyCollection<TodoListItem> Items { get; set; } = new List<TodoListItem>();
    public string NewTodoItemDescription { get; set; } = string.Empty;

    public override void Initialize()
    {
    }

    public record LoadTodoList : IAction;

    public record AddNewItem(string? Text, Temporality Temporality) : IAction;

    public record MarkItemAsDone(Guid ItemId) : IAction;

    public record MarkItemAsToDo(Guid ItemId) : IAction;

    public record FixItemDescription(Guid ItemId, string NewDescription) : IAction;
}