namespace EventSourcedTodoList.Domain.Todo.List;

public record TodoListItem(Guid Id, string Description, bool IsDone)
{
    public bool IsDone { get; private set; } = IsDone;

    public void MarkAsDone() => IsDone = true;

    public void MarkAsToDo() => IsDone = false;
}