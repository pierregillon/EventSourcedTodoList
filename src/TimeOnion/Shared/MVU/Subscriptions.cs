namespace TimeOnion.Shared.MVU;

public class Subscriptions
{
    private readonly List<Subscription> _subscriptions;

    public Subscriptions() => _subscriptions = new List<Subscription>();

    public Subscriptions Add(Type stateType, object stateScope, IBlazorStateComponent component)
    {
        if (!_subscriptions.Any(subscription =>
                subscription.StateType == stateType
                && subscription.StateScope == stateScope
                && subscription.ComponentId == component.Id))
        {
            var subscription = new Subscription(
                component.Id,
                stateType,
                stateScope,
                new WeakReference<IBlazorStateComponent>(component));

            _subscriptions.Add(subscription);
        }

        return this;
    }

    public Subscriptions Remove(IBlazorStateComponent aBlazorStateComponent)
    {
        _subscriptions.RemoveAll(aRecord => aRecord.ComponentId == aBlazorStateComponent.Id);

        return this;
    }

    public async Task ExecuteOnSubscribedComponents<TState>(Func<IBlazorStateComponent, Task> action)
        where TState : IState
    {
        var subscriptions = _subscriptions
            .Where(aRecord => aRecord.StateType == typeof(TState))
            .ToList();

        foreach (var subscription in subscriptions)
        {
            if (subscription.BlazorStateComponentReference.TryGetTarget(out var target))
            {
                await action(target);
            }
        }
    }

    public void ReRenderSubscribers<T>()
    {
        var type = typeof(T);

        ReRenderSubscribers(type);
    }

    public void ReRenderSubscribers(Type stateType)
    {
        var subscriptions = _subscriptions
            .Where(aRecord => aRecord.StateType == stateType)
            .ToList();

        ReRender(subscriptions);
    }

    public void ReRenderSubscribers(Type stateType, object stateScope)
    {
        var subscriptions = _subscriptions
            .Where(subscription => subscription.StateType == stateType)
            .Where(subscription => Equals(subscription.StateScope, stateScope))
            .ToList();

        ReRender(subscriptions);
    }

    private void ReRender(IReadOnlyCollection<Subscription> subscriptions)
    {
        foreach (var subscription in subscriptions)
        {
            if (subscription.BlazorStateComponentReference.TryGetTarget(out var target))
            {
                target.ReRender();
            }
            else
            {
                _subscriptions.Remove(subscription);
            }
        }
    }

    private record Subscription(
        string ComponentId,
        Type StateType,
        object StateScope,
        WeakReference<IBlazorStateComponent> BlazorStateComponentReference
    );
}