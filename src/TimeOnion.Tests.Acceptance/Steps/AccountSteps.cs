using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.GlobalContext;
using TimeOnion.Domain;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class AccountSteps
{
    private readonly TestApplication _application;
    private UserId? _userId;

    public AccountSteps(TestApplication application) => _application = application;

    [Given(@"a user has registered with the email ""(.*)"" and password ""(.*)""")]
    [When(@"I register with the email ""(.*)"" and password ""(.*)""")]
    public async Task WhenIRegisterWithTheEmailAndPassword(string emailAddress, string password) => _userId =
        await _application.Dispatch(() =>
            new RegisterUserCommand(new EmailAddress(emailAddress), new Password(password)));

    [Given(@"I am registered and logged in")]
    [When(@"another user registered and logged in")]
    public async Task GivenIAmRegisteredAndLoggedIn()
    {
        var emailAddress = new EmailAddress($"{Guid.NewGuid()}@test.com");

        var userId = await _application.Dispatch(() => new RegisterUserCommand(
            emailAddress,
            new Password("P@ssw0rd!"))
        );
        
        _application
            .GetService<InMemoryUserContextProvider>()
            .SetUserContext(new UserContext(userId!, emailAddress));
    }

    [Then(@"I am correctly registered")]
    public void ThenIGotANewTokenToUseTheApp() => _userId.Should().NotBeNull();

    [Then(@"I can log in with email ""(.*)"" and password ""(.*)""")]
    public async Task ThenICanLogInWithEmailAndPassword(string emailAddress, string password)
    {
        var query = new GetUserLoginDetailsQuery(new EmailAddress(emailAddress), new UnverifiedPassword(password));
        
        var loginDetails = await _application.Dispatch(query);

        loginDetails.Should().NotBeNull();
    }

    [Given(@"I am disconnected")]
    public void GivenIAmDisconnected()
    {
        _application
            .GetService<InMemoryUserContextProvider>()
            .RemoveUserContext();
    }
}