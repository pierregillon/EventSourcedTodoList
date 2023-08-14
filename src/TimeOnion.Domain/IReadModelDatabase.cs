using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Domain;

public interface IReadModelDatabase
{
    Task<IEnumerable<T>> GetAll<T>() where T : IProjection;
    Task<T?> Find<T>(Predicate<T> predicate) where T : IProjection;
    Task<IReadOnlyCollection<T>> FindAll<T>(Predicate<T> predicate)  where T : IProjection;
    Task Add<T>(T element) where T : IProjection;
    Task Update<T>(Predicate<T> predicate, Func<T, T> update) where T : IProjection;
    Task Delete<T>(Predicate<T> predicate) where T : IProjection;
}

public interface IUserScopedReadModelDatabase
{
    Task<IReadOnlyCollection<T>> GetAll<T>() where T : IUserScopedProjection;
    Task<T?> Find<T>(Predicate<T> predicate) where T : IUserScopedProjection;
    Task<IReadOnlyCollection<T>> FindAll<T>(Predicate<T> predicate) where T : IUserScopedProjection;
}

public interface IUserScopedProjection : IProjection
{
    UserId? UserId { get; }
}

public interface IProjection
{
}
