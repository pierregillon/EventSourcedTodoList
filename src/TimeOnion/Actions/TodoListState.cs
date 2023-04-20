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

    public record CreateNewTodoList : IAction;

    public record AddNewItem(TodoListId ListId, string? Text) : IAction;

    public record MarkItemAsDone(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record MarkItemAsToDo(TodoListId ListId, TodoItemId ItemId) : IAction;

    public record FixItemDescription(TodoListId ListId, TodoItemId ItemId, string NewDescription) : IAction;

    public record RescheduleTodoItem(TodoListId ListId, TodoItemId ItemId, Temporality Temporality) : IAction;

    public record ChangeCurrentTemporality(Temporality Temporality) : IAction;

    public record DeleteItem(TodoListId ListId, TodoItemId ItemId) : IAction;
}