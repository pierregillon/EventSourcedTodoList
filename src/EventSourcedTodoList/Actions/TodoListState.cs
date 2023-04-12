using BlazorState;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public IReadOnlyCollection<TodoListItem> Items { get; set; } = new List<TodoListItem>();


    public override void Initialize()
    {
    }

    public record LoadTodoList : IAction;

    public record AddNewItem(string? Text) : IAction;

    public record MarkItemAsDone(Guid ItemId) : IAction;

    public record MarkItemAsToDo(Guid ItemId) : IAction;
}