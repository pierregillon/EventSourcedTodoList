using MediatR;
using MudBlazor;
using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Shared.MVU.Pipelines;

internal sealed class DisplayExceptionWithSnackbar<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ISnackbar _snackbar;

    public DisplayExceptionWithSnackbar(ISnackbar snackbar)
    {
        _snackbar = snackbar;
        {}
    }

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
        catch (DomainException e)
        {
            ShowSnackbar(e.Message);
            return default!;
        }
        catch (Exception)
        {
            ShowSnackbar("Une erreur non gérée est survenue. Si le problème persiste, contacter votre administrateur.");
            return default!;
        }
    }

    private void ShowSnackbar(string message)
    {
        _snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomCenter;
        _snackbar.Configuration.SnackbarVariant = Variant.Text;
        _snackbar.Configuration.ShowTransitionDuration = 100;
        _snackbar.Configuration.VisibleStateDuration = 5000;
        _snackbar.Add(message, Severity.Error);
    }
}