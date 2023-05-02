using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Domain.Todo.Core.Events;

namespace TimeOnion.Domain.Todo.UseCases;

public record ListTodoListsQuery : IQuery<IReadOnlyCollection<TodoListReadModel>>;

public record TodoListReadModel(TodoListId Id, string Name);

internal class ListTodoListsQueryHandler : IQueryHandler<ListTodoListsQuery, IReadOnlyCollection<TodoListReadModel>>,
    IDomainEventListener<TodoListCreated>,
    IDomainEventListener<TodoListRenamed>,
    IDomainEventListener<TodoListDeleted>
{
    private readonly IReadModelDatabase _database;

    public ListTodoListsQueryHandler(IReadModelDatabase database) => _database = database;

    public async Task<IReadOnlyCollection<TodoListReadModel>> Handle(ListTodoListsQuery query) =>
        (await _database.GetAll<TodoListReadModel>()).ToArray();

    public async Task On(TodoListCreated domainEvent) =>
        await _database.Add(new TodoListReadModel(domainEvent.ListId, domainEvent.Name.Value));

    public async Task On(TodoListRenamed domainEvent) => await _database.Update<TodoListReadModel>(
        list => list.Id == domainEvent.ListId,
        list => list with { Name = domainEvent.NewName.Value }
    );

    public async Task On(TodoListDeleted domainEvent) => await _database.Delete<TodoListReadModel>(
        list => list.Id == domainEvent.ListId
    );
}