using MediatR;
using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Shared.MVU;

namespace TimeOnion.Configuration.GlobalContext;

public sealed class VerifyRequestAuthorization<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUserContextProvider _userContextProvider;

    public VerifyRequestAuthorization(IUserContextProvider userContextProvider) =>
        _userContextProvider = userContextProvider;

    public Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (request is IAction || request is IAnonymousCommand || request is IAnonymousQuery)
        {
            return next();
        }

        if (!_userContextProvider.IsUserContextDefined())
        {
            throw new InvalidOperationException(
                $"You are not authorized to execute the request '{typeof(TRequest).Name}'.");
        }

        return next();
    }
}