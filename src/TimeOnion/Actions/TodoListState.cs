using BlazorState;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Actions;

public class TodoListState : State<TodoListState>
{
    public string NewTodoItemDescription { get; set; } = string.Empty;
    public Temporality CurrentTemporality { get; set; } = Temporality.ThisDay;
    public IEnumerable<TodoListReadModel> TodoLists { get; set; } = Array.Empty<TodoListReadModel>();

    public override void Initialize()
    {
    }

    public record LoadData : IAction;

    public record AddNewItem(string? Text) : IAction;

    public record MarkItemAsDone(TodoItemId ItemId) : IAction;

    public record MarkItemAsToDo(TodoItemId ItemId) : IAction;

    public record FixItemDescription(TodoItemId ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(TodoItemId ItemId, Temporality Temporality) : IAction;

    public record ChangeCurrentTemporality(Temporality Temporality) : IAction;

    public record DeleteItem(TodoItemId ItemId) : IAction;
}