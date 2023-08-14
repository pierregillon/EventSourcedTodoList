using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryDeleted(CategoryId CategoryId) : UserDomainEvent(CategoryId.Value);