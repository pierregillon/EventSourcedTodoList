namespace EventSourcedTodoList.Domain.Todo.List;

public record TodoListItem(Guid Id, string Description, bool IsDone)
{
    public bool IsDone { get; private set; } = IsDone;
    public string Description { get; private set; } = Description;

    public void MarkAsDone() => IsDone = true;

    public void MarkAsToDo() => IsDone = false;

    public void UpdateDescription(ItemDescription itemDescription) => Description = itemDescription.Value;
}