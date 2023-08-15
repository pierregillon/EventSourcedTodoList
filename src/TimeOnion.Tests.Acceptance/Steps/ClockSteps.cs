using NSubstitute;
using TechTalk.SpecFlow;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Tests.Acceptance.Configuration;

namespace TimeOnion.Tests.Acceptance.Steps;

[Binding]
public class ClockSteps
{
    private readonly TestApplication _application;

    public ClockSteps(TestApplication application) => _application = application;

    [Given(@"the current date is (.*)")]
    public void GivenTheCurrentDateIs(DateTime date) => _application.GetService<IClock>().Now().Returns(date);

    [When(@"(.*) hours? passed")]
    public void WhenHourPassed(int hourCount)
    {
        var clock = _application.GetService<IClock>();
        var currentDate = clock.Now();
        clock.Now().Returns(currentDate.AddHours(hourCount));
    }

    [When(@"(.*) days? passed")]
    public void WhenDayPassed(int dayCount)
    {
        var clock = _application.GetService<IClock>();
        var currentDate = clock.Now();
        clock.Now().Returns(currentDate.AddDays(dayCount));
    }

    [When(@"(.*) months? passed")]
    public void WhenMonthPassed(int monthCount)
    {
        var clock = _application.GetService<IClock>();
        var currentDate = clock.Now();
        var newDate = currentDate.AddMonths(monthCount);
        clock.Now().Returns(newDate);
    }
}