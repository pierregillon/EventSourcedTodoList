namespace TimeOnion.Domain.Todo.List;

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons
)
{
    public string Selector { get; set; } = "myid";
    public string Identifier { get; set; } = "myid";
}