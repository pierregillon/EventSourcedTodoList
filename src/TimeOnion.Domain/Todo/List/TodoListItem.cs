namespace TimeOnion.Domain.Todo.List;

public record TodoListItem(Guid Id, string Description, bool IsDone, Temporality Temporality, bool IsDeleted);