using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record CompleteTodoItemCommand(TodoItemId ItemId) : ICommand;

internal class CompleteTodoItemCommandHandler : ICommandHandler<CompleteTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public CompleteTodoItemCommandHandler(ITodoListRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(CompleteTodoItemCommand command)
    {
        var todoList = await _repository.Get();

        todoList.Complete(command.ItemId);

        await _repository.Save(todoList);
    }
}