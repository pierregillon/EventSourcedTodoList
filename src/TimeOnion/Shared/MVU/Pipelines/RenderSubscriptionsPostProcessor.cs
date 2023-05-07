using MediatR.Pipeline;

namespace TimeOnion.Shared.MVU.Pipelines;

internal class RenderSubscriptionsPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Subscriptions _subscriptions;

    public RenderSubscriptionsPostProcessor(Subscriptions aSubscriptions) => _subscriptions = aSubscriptions;

    public Task Process(TRequest request, TResponse aResponse, CancellationToken aCancellationToken)
    {
        if (request is not IAction action)
        {
            return Task.CompletedTask;
        }

        var actionInterface = GetActionInterface(action);

        if (actionInterface is null)
        {
            return Task.CompletedTask;
        }

        var stateType = actionInterface.GetGenericArguments().Single();

        if (action is IActionOnScopedState actionOnScopedState)
        {
            _subscriptions.ReRenderSubscribers(stateType, actionOnScopedState.Scope);
        }
        else
        {
            _subscriptions.ReRenderSubscribers(stateType);
        }

        return Task.CompletedTask;
    }

    private static Type? GetActionInterface(IAction action) => action
        .GetType()
        .FindInterfaces(
            (interfaceType, _) => interfaceType.IsGenericType
                && interfaceType.GetGenericTypeDefinition() == typeof(IAction<>), null)
        .SingleOrDefault();
}