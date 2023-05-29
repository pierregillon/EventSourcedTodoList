using TimeOnion.Shared.MVU;

namespace TimeOnion.Pages.Authentication.Register;

public record RegisterState(
    string Email,
    string Password,
    string PasswordRepeat,
    bool IsLoading
) : IState
{
    public static RegisterState Initialize() => new(string.Empty, string.Empty, string.Empty, false);
}