using MediatR.Pipeline;

namespace TimeOnion.Shared.MVU.Pipelines;

internal class RenderSubscriptionsPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Subscriptions _subscriptions;

    public RenderSubscriptionsPostProcessor(Subscriptions aSubscriptions) => _subscriptions = aSubscriptions;

    public Task Process(TRequest aRequest, TResponse aResponse, CancellationToken aCancellationToken)
    {
        if (aRequest is not IAction)
        {
            return Task.CompletedTask;
        }

        var actionInterface = aRequest
            .GetType()
            .FindInterfaces(
                (interfaceType, _) => interfaceType.IsGenericType
                    && interfaceType.GetGenericTypeDefinition() == typeof(IAction<>), null)
            .SingleOrDefault();

        if (actionInterface is not null)
        {
            var stateType = actionInterface.GetGenericArguments().Single();

            _subscriptions.ReRenderSubscribers(stateType);
        }

        return Task.CompletedTask;
    }
}