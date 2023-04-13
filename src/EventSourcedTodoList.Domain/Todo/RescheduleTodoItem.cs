using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Domain.Todo.List;

namespace EventSourcedTodoList.Domain.Todo;

public record RescheduleTodoItemCommand(TodoItemId TodoItemId, Temporality AnotherTemporality) : ICommand;

internal class RescheduleTodoItemCommandHandler : ICommandHandler<RescheduleTodoItemCommand>
{
    private readonly ITodoListRepository _repository;

    public RescheduleTodoItemCommandHandler(ITodoListRepository repository) => _repository = repository;

    public async Task Handle(RescheduleTodoItemCommand command)
    {
        var todoList = await _repository.Get();

        todoList.Reschedule(command.TodoItemId, command.AnotherTemporality);

        await _repository.Save(todoList);
    }
}