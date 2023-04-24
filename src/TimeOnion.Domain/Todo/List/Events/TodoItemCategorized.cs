namespace TimeOnion.Domain.Todo.List.Events;

public record TodoItemCategorized(
    TodoListId Id,
    TodoItemId ItemId,
    CategoryId? PreviousCategoryId,
    CategoryId NewCategoryId
) : TodoListDomainEvent(Id);