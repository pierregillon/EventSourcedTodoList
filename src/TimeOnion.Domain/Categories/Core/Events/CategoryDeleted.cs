namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryDeleted(CategoryId CategoryId) : CategoryDomainEvent(CategoryId);