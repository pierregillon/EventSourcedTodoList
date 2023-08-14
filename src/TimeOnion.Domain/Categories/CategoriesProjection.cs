using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Categories.Core.Events;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Categories;

internal record CategoriesProjection(IReadModelDatabase Database) : 
    IDomainEventListener<CategoryCreated>,
    IDomainEventListener<CategoryRenamed>,
    IDomainEventListener<CategoryDeleted>
{
    public async Task On(CategoryCreated domainEvent)
    {
        var item = new CategoryProjectionItem(
            domainEvent.CategoryId,
            domainEvent.Name.Value,
            domainEvent.ListId,
            domainEvent.As<IUserDomainEvent>().UserId
        );
        await Database.Add(item);
    }

    public async Task On(CategoryRenamed domainEvent) => await Database.Update<CategoryProjectionItem>(
        category => category.Id == domainEvent.CategoryId,
        category => category with { Name = domainEvent.NewName.Value }
    );

    public async Task On(CategoryDeleted domainEvent) => await Database.Delete<CategoryProjectionItem>(
        category => category.Id == domainEvent.CategoryId
    );
}

public record CategoryProjectionItem(
    CategoryId Id,
    string Name,
    TodoListId ListId,
    UserId UserId
) : IUserScopedProjection;
