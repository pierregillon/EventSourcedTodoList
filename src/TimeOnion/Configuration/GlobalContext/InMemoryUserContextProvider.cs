using TimeOnion.Domain;

namespace TimeOnion.Configuration.GlobalContext;

public class InMemoryUserContextProvider : IUserContextProvider
{
    private UserContext? _userContext;

    public void SetUserContext(UserContext userContext) => _userContext = userContext;

    public UserContext GetUserContext() =>
        _userContext ?? throw new InvalidOperationException("No user context defined");

    public bool IsUserContextDefined() => _userContext is not null;

    public void RemoveUserContext() =>
        _userContext = null;
}