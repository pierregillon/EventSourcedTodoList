using BlazorState;

namespace EventSourcedTodoList.Actions;

public class TodoListState : State<TodoListState>
{
    public List<TodoListItem> Items { get; } = new();


    public override void Initialize()
    {
    }

    public record AddNewItemCommand(string? Text) : IAction;
}