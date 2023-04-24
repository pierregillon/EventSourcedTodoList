namespace TimeOnion.Domain.Todo.List;

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    TimeHorizons TimeHorizons,
    CategoryId? CategoryId
);