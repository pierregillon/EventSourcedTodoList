namespace EventSourcedTodoList.Domain.Todo.List;

public record TodoListId(Guid Id)
{
    public static readonly TodoListId Unique = new(Guid.Empty);
}