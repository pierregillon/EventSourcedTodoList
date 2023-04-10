namespace EventSourcedTodoList.Domain.Todo.List;

public record TodoListItem(Guid Id, string Description, bool IsCompleted)
{
    public bool IsCompleted { get; private set; } = IsCompleted;

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }
}