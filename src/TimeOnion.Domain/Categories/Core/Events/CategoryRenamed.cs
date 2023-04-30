namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryRenamed(CategoryId Id, CategoryName? PreviousName, CategoryName NewName) : CategoryDomainEvent(Id);