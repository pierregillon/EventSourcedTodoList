using MudBlazor;

namespace TimeOnion.Shared;

public static class DialogServiceExtensions
{
    public static async Task<bool> ShowConfirmationDialog(
        this IDialogService dialogService,
        string title,
        string message
    )
    {
        var options = new DialogOptions { CloseOnEscapeKey = true, DisableBackdropClick = true, CloseButton = true};
        var parameters = new DialogParameters
        {
            { "ConfirmationMessage", message }
        };
        var dialog = await dialogService.ShowAsync<ConfirmationDialog>(title, parameters, options);
        var result = await dialog.Result;

        return !result.Canceled && (bool)result.Data;
    }
}