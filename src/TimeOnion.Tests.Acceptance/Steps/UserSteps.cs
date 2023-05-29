using FluentAssertions;
using TechTalk.SpecFlow;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class UserSteps
{
    private readonly TestApplication _application;
    private UserId? _userId;

    public UserSteps(TestApplication application) => _application = application;

    [Given(@"a user has registered with the email ""(.*)"" and password ""(.*)""")]
    [When(@"I register with the email ""(.*)"" and password ""(.*)""")]
    public async Task WhenIRegisterWithTheEmailAndPassword(string emailAddress, string password) => _userId =
        await _application.Dispatch(() =>
            new RegisterUserCommand(new EmailAddress(emailAddress), new Password(password)));

    [Then(@"I am correctly registered")]
    public void ThenIGotANewTokenToUseTheApp() => _userId.Should().NotBeNull();

    [Then(@"the user with email ""(.*)"" and password ""(.*)"" exists")]
    public async Task ThenTheUserWithEmailAndPasswordExists(string emailAddress, string password)
    {
        var query = new GetUserLoginDetailsQuery(new EmailAddress(emailAddress), new UnverifiedPassword(password));
        
        var loginDetails = await _application.Dispatch(query);

        loginDetails.Should().NotBeNull();
    }
}