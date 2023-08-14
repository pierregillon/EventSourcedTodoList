using System.Collections;
using System.Collections.Concurrent;
using TimeOnion.Domain;

namespace TimeOnion.Infrastructure;

public class InMemoryReadModelDatabase : IReadModelDatabase
{
    private readonly IDictionary<Type, IList> _elements = new ConcurrentDictionary<Type, IList>();

    public async Task<IEnumerable<T>> GetAll<T>() where T : IProjection
    {
        await Task.Delay(0);

        if (_elements.TryGetValue(typeof(T), out var list))
        {
            return list.Cast<T>();
        }

        return Enumerable.Empty<T>();
    }

    public Task<T?> Find<T>(Predicate<T> predicate) where T : IProjection
    {
        if (_elements.TryGetValue(typeof(T), out var list))
        {
            var result = list.Cast<T>().FirstOrDefault(item => predicate(item));
            return Task.FromResult(result);
        }

        return Task.FromResult(default(T?));
    }
    
    public Task<IReadOnlyCollection<T>> FindAll<T>(Predicate<T> predicate) where T : IProjection
    {
        if (_elements.TryGetValue(typeof(T), out var list))
        {
            IReadOnlyCollection<T> result = list.Cast<T>().Where(item => predicate(item)).ToArray();
            return Task.FromResult(result);
        }

        return Task.FromResult((IReadOnlyCollection<T>)new List<T>());
    }

    public Task Add<T>(T element) where T : IProjection
    {
        if (!_elements.TryGetValue(typeof(T), out var list))
        {
            list = new List<T>();
            _elements.Add(typeof(T), list);
        }

        list.Add(element);

        return Task.CompletedTask;
    }

    public Task Update<T>(Predicate<T> predicate, Func<T, T> update) where T : IProjection
    {
        if (!_elements.TryGetValue(typeof(T), out var list))
        {
            throw new InvalidOperationException("Cannot update element: list was not found");
        }

        for (var index = 0; index < list.Count; index++)
        {
            var element = (T)list[index]!;
            if (!predicate(element))
            {
                continue;
            }

            list.Insert(index, update(element));
            list.Remove(element);
            break;
        }

        return Task.CompletedTask;
    }

    public Task Delete<T>(Predicate<T> predicate) where T : IProjection
    {
        if (!_elements.TryGetValue(typeof(T), out var list))
        {
            throw new InvalidOperationException("Cannot update element: list was not found");
        }

        for (var index = 0; index < list.Count; index++)
        {
            var element = (T)list[index]!;
            if (predicate(element))
            {
                list.Remove(element);
            }
        }

        return Task.CompletedTask;
    }
}