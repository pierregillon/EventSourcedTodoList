using System.Collections;
using System.Collections.Concurrent;
using EventSourcedTodoList.Domain.Todo;

namespace EventSourcedTodoList.Infrastructure;

public class InMemoryReadModelDatabase : IReadModelDatabase
{
    private readonly IDictionary<Type, IList> _elements = new ConcurrentDictionary<Type, IList>();

    public async Task<IEnumerable<T>> GetAll<T>()
    {
        await Task.Delay(0);

        if (_elements.TryGetValue(typeof(T), out var list)) return list.Cast<T>();

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
}