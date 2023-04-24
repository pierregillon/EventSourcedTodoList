namespace TimeOnion.Domain.Categories.Core;

public record CategoryCreated(CategoryId Id, CategoryName Name) : CategoryDomainEvent(Id);