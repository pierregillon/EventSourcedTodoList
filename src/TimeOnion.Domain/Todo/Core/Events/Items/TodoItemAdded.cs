using TimeOnion.Domain.Categories.Core;

namespace TimeOnion.Domain.Todo.Core.Events.Items;

public record TodoItemAdded(
    TodoListId ListId,
    TodoItemId ItemId,
    TodoItemDescription Description,
    TimeHorizons TimeHorizon,
    CategoryId? CategoryId,
    TodoItemId? AboveItemId
) : TodoListDomainEvent(ListId);