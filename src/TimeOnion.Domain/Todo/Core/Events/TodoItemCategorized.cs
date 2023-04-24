using TimeOnion.Domain.Categories;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemCategorized(
    TodoListId Id,
    TodoItemId ItemId,
    CategoryId? PreviousCategoryId,
    CategoryId NewCategoryId
) : TodoListDomainEvent(Id);