using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement;

namespace TimeOnion.Configuration.Authentication;

public class Authenticator
{
    private readonly JwtTokenBuilder _jwtTokenBuilder;
    private readonly IQueryDispatcher _queryDispatcher;

    public Authenticator(JwtTokenBuilder jwtTokenBuilder, IQueryDispatcher queryDispatcher)
    {
        _jwtTokenBuilder = jwtTokenBuilder;
        _queryDispatcher = queryDispatcher;
    }

    public async Task<JwtToken> Authenticate(string email, string password)
    {
        var loginDetails = await _queryDispatcher.Dispatch(new GetUserLoginDetailsQuery(email, password));

        if (loginDetails is null)
        {
            throw new InvalidOperationException("Unable to login");
        }

        return _jwtTokenBuilder.Build(loginDetails.UserId, loginDetails.Email);
    }
}