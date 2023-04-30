using TimeOnion.Domain.Categories;
using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoItemCategorized(
    TodoListId Id,
    TodoItemId ItemId,
    CategoryId? PreviousCategoryId,
    CategoryId NewCategoryId
) : TodoListDomainEvent(Id);