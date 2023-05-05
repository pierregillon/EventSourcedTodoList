using System.Collections.Concurrent;
using System.Reflection;

namespace TimeOnion.Shared.MVU;

public class InMemoryStore : IStore
{
    private readonly IDictionary<Type, IState> _states = new ConcurrentDictionary<Type, IState>();

    public T GetState<T>() where T : IState
    {
        if (!_states.TryGetValue(typeof(T), out var state))
        {
            state = CreateDefaultState<T>();
            _states.Add(typeof(T), state);
        }

        return (T)state;
    }

    private static IState CreateDefaultState<T>() where T : IState
    {
        var initializeMethod = typeof(T).GetMethod("Initialize", BindingFlags.Static | BindingFlags.Public);
        if (initializeMethod is null)
        {
            throw new InvalidOperationException(
                $"A static Initialize() method should be defined on type {typeof(T).Name} in order to have the initial state.");
        }

        return (T)initializeMethod.Invoke(null, Array.Empty<object?>())!;
    }

    public void SetState<T>(T state) where T : IState => _states[typeof(T)] = state;
}