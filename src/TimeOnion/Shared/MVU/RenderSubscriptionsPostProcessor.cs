using MediatR.Pipeline;

namespace TimeOnion.Shared.MVU;

internal class RenderSubscriptionsPostProcessor<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly Subscriptions _subscriptions;

    public RenderSubscriptionsPostProcessor(Subscriptions aSubscriptions) => _subscriptions = aSubscriptions;

    public Task Process(TRequest aRequest, TResponse aResponse, CancellationToken aCancellationToken)
    {
        if (aRequest is IAction)
        {
            _subscriptions.ReRenderSubscribers(typeof(TRequest).DeclaringType!);
        }

        return Task.CompletedTask;
    }
}