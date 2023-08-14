using Bunit;
using FluentAssertions;
using MudBlazor;
using NSubstitute.ReceivedExtensions;
using TimeOnion.Domain.UserManagement;
using TimeOnion.Domain.UserManagement.Core;
using TimeOnion.Pages.Authentication.Register;

namespace TimeOnion.WebApp.Tests.Unit;

public class RegisterComponentTests : ComponentTestBase
{
    [Fact]
    public void Renders_3_untouched_inputs()
    {
        var component = RenderComponent<Register>();

        var inputs = component.FindComponents<MudTextField<string>>();

        inputs.Should().HaveCount(3);
        inputs.ElementAt(0).Instance.Label.Should().Be("Email");
        inputs.ElementAt(1).Instance.Label.Should().Be("Mot de passe");
        inputs.ElementAt(2).Instance.Label.Should().Be("Répétez le mot de passe");
        inputs.Any(x => x.Instance.Touched).Should().Be(false);

        component.FindAll(".mud-input-error").Should().BeEmpty();
        component.Markup.Should().NotContain("L'email est requis");
        component.Markup.Should().NotContain("Le mot de passe est requis");
    }

    [Fact]
    public async Task Registering_posts_command()
    {
        var component = RenderComponent<Register>();

        var inputs = component.FindAll("input");

        inputs.ElementAt(0).Input("test@company.com");
        inputs.ElementAt(1).Input("P@ssw0rd!");
        inputs.ElementAt(2).Input("P@ssw0rd!");

        await SubmitFormFromFirstButton(component);

        await CommandDispatcher
            .Received(Quantity.Exactly(1))
            .Dispatch(
                new RegisterUserCommand(new EmailAddress("test@company.com"), new Password("P@ssw0rd!"))
            );
    }
}