using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryCreated(
    CategoryId CategoryId,
    CategoryName Name,
    TodoListId ListId
) : UserDomainEvent(CategoryId.Value);