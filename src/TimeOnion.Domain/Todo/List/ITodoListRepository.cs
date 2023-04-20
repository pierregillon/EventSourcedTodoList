namespace TimeOnion.Domain.Todo.List;

public interface ITodoListRepository
{
    Task<TodoList> Get(TodoListId Id);
    Task Save(TodoList todoList);
}