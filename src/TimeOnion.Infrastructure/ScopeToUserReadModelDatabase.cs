using TimeOnion.Domain;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Infrastructure;

public class ScopeToUserReadModelDatabase : IUserScopedReadModelDatabase
{
    private readonly IReadModelDatabase _decorated;
    private readonly IUserContextProvider _userContextProvider;

    public ScopeToUserReadModelDatabase(IReadModelDatabase decorated, IUserContextProvider userContextProvider)
    {
        _decorated = decorated;
        _userContextProvider = userContextProvider;
    }

    public async Task<IReadOnlyCollection<T>> GetAll<T>() where T : IUserScopedProjection
    {
        return (await _decorated.GetAll<T>())
            .ScopeToUserId(_userContextProvider.GetUserContext().UserId)
            .ToArray();
    }

    public Task<T?> Find<T>(Predicate<T> predicate) where T : IUserScopedProjection =>
        _decorated.Find(GetPredicate(predicate));

    public Task<IReadOnlyCollection<T>> FindAll<T>(Predicate<T> predicate) where T : IUserScopedProjection =>
        _decorated.FindAll(GetPredicate(predicate));

    private Predicate<T> GetPredicate<T>(Predicate<T> predicate)
    {
        var userId = _userContextProvider.GetUserContext().UserId;
        return item => predicate(item) && ((IUserScopedProjection)item!).UserId == userId;
    }
}

public static class EnumerableExtensions
{
    public static IEnumerable<T> ScopeToUserId<T>(this IEnumerable<T> enumerable, UserId userId)
        where T : IUserScopedProjection
        => enumerable.Where(x => x.UserId == userId);
}