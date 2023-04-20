namespace TimeOnion.Domain.Todo.List;

public record TodoListReadModel(TodoListId Id, string Name, IReadOnlyCollection<TodoListItemReadModel> Items);