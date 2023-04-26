using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories.Core;

public record CategoryCreated(CategoryId Id, CategoryName Name, TodoListId ListId) : CategoryDomainEvent(Id);