namespace TimeOnion.Domain.Todo.List;

public record TodoListItemReadModel(
    TodoItemId Id,
    string Description,
    bool IsDone,
    Temporality Temporality,
    bool IsDeleted
);

public record TodoListReadModel(TodoListId Id, string Name, IReadOnlyCollection<TodoListItemReadModel> Items);