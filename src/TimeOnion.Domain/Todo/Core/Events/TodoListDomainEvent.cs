using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Todo.Core.Events;

public record TodoListDomainEvent(TodoListId ListId) : IDomainEvent
{
    Guid IDomainEvent.AggregateId => ListId.Value;
    int IDomainEvent.Version { get; set; }
    DateTime IDomainEvent.CreatedAt { get; set; }
}