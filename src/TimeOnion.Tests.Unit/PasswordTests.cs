using FluentAssertions;
using TimeOnion.Domain.UserManagement.Core;

namespace TimeOnion.Tests.Unit;

public class PasswordTests
{
    [Theory]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaaaaaa")]
    public void A_password_must_be_at_least_of_length_8(string toShortPassword)
    {
        var action = () => new Password(toShortPassword);

        action
            .Should()
            .Throw<TooWeakPasswordException>()
            .WithMessage("Password must be at least of length 8");
    }

    [Theory]
    [InlineData("abcdefgh")]
    public void Password_must_contain_at_least_one_capital_letter(string passwordWithoutCapitalLetter)
    {
        var action = () => new Password(passwordWithoutCapitalLetter);

        action
            .Should()
            .Throw<TooWeakPasswordException>()
            .WithMessage("Password must contain at least one capital letter");
    }

    [Theory]
    [InlineData("ABCDEFGH")]
    public void Password_must_contain_at_least_one_lowercase_letter(string passwordWithoutLowercase)
    {
        var action = () => new Password(passwordWithoutLowercase);

        action
            .Should()
            .Throw<TooWeakPasswordException>()
            .WithMessage("Password must contain at least one lowercase letter");
    }

    [Theory]
    [InlineData("Abcdefgh")]
    public void Password_must_contain_at_least_one_digit(string passwordWithoutDigit)
    {
        var action = () => new Password(passwordWithoutDigit);

        action
            .Should()
            .Throw<TooWeakPasswordException>()
            .WithMessage("Password must contain at least one digit");
    }
}