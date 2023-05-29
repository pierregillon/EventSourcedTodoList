using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.Authentication;

public record LoginState(string RedirectUrl, string Email, string Password) : IState
{
    public static LoginState Initialize() => new(string.Empty, string.Empty, string.Empty);
}