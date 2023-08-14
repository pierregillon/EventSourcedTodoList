using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain.Categories.Core.Events;

public record CategoryRenamed(
    CategoryId CategoryId,
    CategoryName? PreviousName,
    CategoryName NewName
) : UserDomainEvent(CategoryId.Value);