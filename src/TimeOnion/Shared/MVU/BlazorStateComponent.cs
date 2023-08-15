using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using TimeOnion.Configuration.Blazor;

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
    [Inject] private IActionDispatcher ActionDispatcher { get; set; } = default!;
    [Inject] private IStore Store { get; set; } = default!;
    [Inject] private Subscriptions Subscriptions { get; set; } = default!;

    protected Task Execute(IAction action) => ActionDispatcher.Dispatch(action);

    protected T GetState<T>() where T : IState => GetState<T>(DefaultScope.Value);

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

    protected virtual Task OnAfterInitialRenderAsync() => Task.CompletedTask;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await OnAfterInitialRenderAsync();
        }

        await base.OnAfterRenderAsync(firstRender);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        var initialValues = GetInitialParameterValues().OrderBy(x => x.Key);

        await base.SetParametersAsync(parameters);

        var newValues = parameters.ToDictionary().OrderBy(x => x.Key);

        List<ChangedParameters> changedParameters =
            (from element in initialValues.Zip(newValues)
                where !Equals(element.First.Value, element.Second.Value)
                select new ChangedParameters(element.First.Key, element.First.Value, element.Second.Value)
            ).ToList();

        if (changedParameters.Any())
        {
            await OnParametersChangedAsync(changedParameters);
        }
    }

    private Dictionary<string, object?> GetInitialParameterValues() => GetType()
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(x => x.GetCustomAttribute<ParameterAttribute>() != null)
        .ToDictionary(x => x.Name, x => x.GetValue(this));

    protected virtual Task OnParametersChangedAsync(IReadOnlyCollection<ChangedParameters> parameters) =>
        Task.CompletedTask;
}

public record ChangedParameters(string ParameterName, object? PreviousValue, object NewValue);