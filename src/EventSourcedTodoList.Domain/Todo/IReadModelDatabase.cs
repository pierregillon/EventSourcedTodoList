namespace EventSourcedTodoList.Domain.Todo;

public interface IReadModelDatabase
{
    Task<IEnumerable<T>> GetAll<T>();
    Task Add<T>(T element);
}