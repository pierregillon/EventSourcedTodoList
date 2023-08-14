using Bunit;
using NSubstitute.ReceivedExtensions;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Pages.Authentication.Login;

namespace TimeOnion.WebApp.Tests.Unit;

public class LoginComponentTests : ComponentTestBase
{
    [Fact]
    public async Task Logging_in_posts_command_to_api()
    {
        var component = RenderComponent<Login>();
        var inputs = component.FindAll("input");

        inputs.ElementAt(0).Input("test@company.com");
        inputs.ElementAt(1).Input("P@ssw0rd!");
        component.FindAll("button").Last().Click();

        await QueryDispatcher
            .Received(Quantity.Exactly(1))
            .Dispatch(new GetUserLoginDetailsQuery(
                new EmailAddress("test@company.com"),
                new UnverifiedPassword("P@ssw0rd!")
            ));
    }
}