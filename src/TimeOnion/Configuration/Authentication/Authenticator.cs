using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Configuration.Authentication;

public class Authenticator
{
    private readonly JwtTokenBuilder _jwtTokenBuilder;
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public Authenticator(
        JwtTokenBuilder jwtTokenBuilder,
        IQueryDispatcher queryDispatcher,
        ICommandDispatcher commandDispatcher
    )
    {
        _jwtTokenBuilder = jwtTokenBuilder;
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    public async Task<JwtToken> Authenticate(string email, string password)
    {
        var query = new GetUserLoginDetailsQuery(new EmailAddress(email), new UnverifiedPassword(password));
        
        var loginDetails = await _queryDispatcher.Dispatch(query);

        if (loginDetails is null || !loginDetails.IsPasswordValid)
        {
            throw new BadLoginCredentialsException("Unable to find a user with email and password");
        }

        return _jwtTokenBuilder.Build(loginDetails.UserId, loginDetails.Email);
    }

    public async Task<JwtToken> Register(string email, string password)
    {
        var command = new RegisterUserCommand(
            new EmailAddress(email),
            new Password(password)
        );

        var userId = await _commandDispatcher.Dispatch(command);

        return _jwtTokenBuilder.Build(userId, email);
    }
}

public class BadLoginCredentialsException : DomainException
{
    public BadLoginCredentialsException(string message) : base(message)
    {
        
    }
}