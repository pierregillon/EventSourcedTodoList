namespace TimeOnion.Domain.Todo.Core;

public interface ITodoListRepository
{
    Task<TodoList> Get(TodoListId Id);
    Task Save(TodoList todoList);
}