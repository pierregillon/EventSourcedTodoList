namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryRenamed
    (CategoryId CategoryId, CategoryName? PreviousName, CategoryName NewName) : CategoryDomainEvent(CategoryId);