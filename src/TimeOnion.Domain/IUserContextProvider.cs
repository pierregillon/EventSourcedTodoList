namespace TimeOnion.Domain;

public interface IUserContextProvider
{
    UserContext GetUserContext();
    bool IsUserContextDefined();
}