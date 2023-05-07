using MediatR;

namespace TimeOnion.Shared.MVU.Pipelines;

internal sealed class LogExceptionPreProcessor<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LogExceptionPreProcessor<TRequest, TResponse>> _logger;

    public LogExceptionPreProcessor(ILogger<LogExceptionPreProcessor<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (request is not IAction)
        {
            return await next();
        }

        try
        {
            return await next();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"An error occurred while processing the request '{typeof(TRequest).Name}'.");
            throw;
        }
    }
}