namespace EventSourcedTodoList.Domain.Todo.List;

public interface ITodoListRepository
{
    Task<TodoList> Get();
    Task Save(TodoList todoList);
}