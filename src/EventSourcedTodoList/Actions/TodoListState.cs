using BlazorState;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public IReadOnlyCollection<TodoListItem> Items { get; set; } = new List<TodoListItem>();


    public override void Initialize()
    {
    }

    public record AddNewItem(string? Text) : IAction;
}