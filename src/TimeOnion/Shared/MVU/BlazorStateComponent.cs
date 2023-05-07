using System.Collections.Concurrent;
using MediatR;
using Microsoft.AspNetCore.Components;

namespace TimeOnion.Shared.MVU;

public class BlazorStateComponent : ComponentBase, IBlazorStateComponent, IDisposable
{
    private static readonly ConcurrentDictionary<string, int> InstanceCounts = new();

    public BlazorStateComponent()
    {
        var name = GetType().Name;
        var count = InstanceCounts.AddOrUpdate(name, 1, (aKey, aValue) => aValue + 1);

        Id = $"{name}-{count}";
    }

    public string Id { get; }
    [Inject] private IMediator Mediator { get; set; } = default!;
    [Inject] private IStore Store { get; set; } = default!;
    [Inject] private Subscriptions Subscriptions { get; set; } = default!;
    protected Task Execute(IAction action) => Mediator.Send(action);

    protected T GetState<T>() where T : IState
    {
        var stateType = typeof(T);
        Subscriptions.Add(stateType, Subscriptions.DefaultScope, this);
        return Store.GetState<T>(Subscriptions.DefaultScope);
    }

    protected T GetState<T>(object scope) where T : IState
    {
        var stateType = typeof(T);
        Subscriptions.Add(stateType, scope, this);
        return Store.GetState<T>(scope);
    }

    public void ReRender() => InvokeAsync(StateHasChanged);

    public virtual void Dispose()
    {
        Subscriptions.Remove(this);
        GC.SuppressFinalize(this);
    }
}