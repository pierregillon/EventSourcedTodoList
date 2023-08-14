using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain.Todo.Projections;

public record TodoListProjection (IReadModelDatabase Database):
    IDomainEventListener<TodoListCreated>,
    IDomainEventListener<TodoListRenamed>,
    IDomainEventListener<TodoListDeleted>
{
    public async Task On(TodoListCreated domainEvent)
    {
        var item = new TodoListProjectItem(domainEvent.ListId,
            domainEvent.Name.Value,
            domainEvent.As<IUserDomainEvent>().UserId
        );
        await Database.Add(item);
    }

    public async Task On(TodoListRenamed domainEvent) => await Database.Update<TodoListProjectItem>(
        list => list.Id == domainEvent.ListId,
        list => list with { Name = domainEvent.NewName.Value }
    );

    public async Task On(TodoListDeleted domainEvent) => await Database.Delete<TodoListProjectItem>(
        list => list.Id == domainEvent.ListId
    );
}

internal record TodoListProjectItem(TodoListId Id, string Name, UserId UserId) : IUserScopedProjection;
