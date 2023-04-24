using System.Collections;
using System.Collections.Concurrent;
using TimeOnion.Domain;

namespace TimeOnion.Infrastructure;

public class InMemoryReadModelDatabase : IReadModelDatabase
{
    private readonly IDictionary<Type, IList> _elements = new ConcurrentDictionary<Type, IList>();

    public async Task<IEnumerable<T>> GetAll<T>()
    {
        await Task.Delay(0);

        if (_elements.TryGetValue(typeof(T), out var list))
        {
            return list.Cast<T>();
        }

        return Enumerable.Empty<T>();
    }

    public Task Add<T>(T element)
    {
        if (!_elements.TryGetValue(typeof(T), out var list))
        {
            list = new List<T>();
            _elements.Add(typeof(T), list);
        }

        list.Add(element);

        return Task.CompletedTask;
    }

    public Task Update<T>(Predicate<T> predicate, Func<T, T> update)
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

    public Task Delete<T>(Predicate<T> predicate)
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