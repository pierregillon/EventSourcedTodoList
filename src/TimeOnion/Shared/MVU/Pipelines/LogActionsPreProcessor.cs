using System.Text.Json;
using MediatR;

namespace TimeOnion.Shared.MVU.Pipelines;

internal sealed class LogActionsPreProcessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LogActionsPreProcessor<TRequest, TResponse>> _logger;

    public LogActionsPreProcessor(ILogger<LogActionsPreProcessor<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (request is IAction action)
        {
            var actionType = action.GetType();
            _logger.LogInformation(actionType.Name + " : " +JsonSerializer.Serialize(action, actionType));
        }

        return await next();
    }
}