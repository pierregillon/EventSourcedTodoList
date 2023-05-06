namespace TimeOnion.Shared.MVU;

public class Subscriptions
{
    private readonly List<Subscription> _subscriptions;

    public Subscriptions() => _subscriptions = new List<Subscription>();

    public Subscriptions Add<T>(IBlazorStateComponent aBlazorStateComponent)
    {
        var type = typeof(T);
        return Add(type, aBlazorStateComponent);
    }

    public Subscriptions Add(Type aType, IBlazorStateComponent aBlazorStateComponent)
    {
        if (!_subscriptions.Any(subscription =>
                subscription.StateType == aType && subscription.ComponentId == aBlazorStateComponent.Id))
        {
            var subscription = new Subscription(
                aType,
                aBlazorStateComponent.Id,
                new WeakReference<IBlazorStateComponent>(aBlazorStateComponent));

            _subscriptions.Add(subscription);
        }

        return this;
    }

    public Subscriptions Remove(IBlazorStateComponent aBlazorStateComponent)
    {
        _subscriptions.RemoveAll(aRecord => aRecord.ComponentId == aBlazorStateComponent.Id);

        return this;
    }

    public void ReRenderSubscribers<T>()
    {
        var type = typeof(T);

        ReRenderSubscribers(type);
    }

    public void ReRenderSubscribers(Type stateType)
    {
        var subscriptions = _subscriptions.Where(aRecord => aRecord.StateType == stateType);
        
        foreach (var subscription in subscriptions.ToList())
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

    private readonly struct Subscription : IEquatable<Subscription>
    {
        public WeakReference<IBlazorStateComponent> BlazorStateComponentReference { get; }

        public string ComponentId { get; }

        public Type StateType { get; }

        public Subscription(
            Type aStateType,
            string aComponentId,
            WeakReference<IBlazorStateComponent> aBlazorStateComponentReference
        )
        {
            StateType = aStateType;
            ComponentId = aComponentId;
            BlazorStateComponentReference = aBlazorStateComponentReference;
        }

        public static bool operator !=(Subscription aLeftSubscription, Subscription aRightSubscription) =>
            !(aLeftSubscription == aRightSubscription);

        public static bool operator ==(Subscription aLeftSubscription, Subscription aRightSubscription) =>
            aLeftSubscription.Equals(aRightSubscription);

        public bool Equals(Subscription aSubscription) =>
            EqualityComparer<Type>.Default.Equals(StateType, aSubscription.StateType)
            && ComponentId == aSubscription.ComponentId
            && EqualityComparer<WeakReference<IBlazorStateComponent>>.Default.Equals(BlazorStateComponentReference,
                aSubscription.BlazorStateComponentReference);

        public override bool Equals(object? aObject) => aObject is not null && Equals((Subscription)aObject);

        public override int GetHashCode() => ComponentId.GetHashCode();
    }
}