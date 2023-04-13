using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record FixItemDescriptionCommand(TodoItemId TodoItemId, ItemDescription NewItemDescription) : ICommand;

public class FixItemDescriptionCommandHandler : ICommandHandler<FixItemDescriptionCommand>
{
    private readonly ITodoListRepository _repository;

    public FixItemDescriptionCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(FixItemDescriptionCommand command)
    {
        var todoList = await _repository.Get();

        todoList.FixItemDescription(command.TodoItemId, command.NewItemDescription);

        await _repository.Save(todoList);
    }
}