namespace TimeOnion.Domain.Todo.List;

public record TodoListItemReadModel(
    TodoItemId Id,
    TodoListId ListId,
    string Description,
    bool IsDone,
    Temporality Temporality
);

public record TodoListReadModel(TodoListId Id, string Name, IReadOnlyCollection<TodoListItemReadModel> Items);