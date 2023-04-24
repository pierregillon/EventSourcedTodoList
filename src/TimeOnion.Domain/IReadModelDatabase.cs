namespace TimeOnion.Domain;

public interface IReadModelDatabase
{
    Task<IEnumerable<T>> GetAll<T>();
    Task Add<T>(T element);
    Task Update<T>(Predicate<T> predicate, Func<T, T> update);
    Task Delete<T>(Predicate<T> predicate);
}