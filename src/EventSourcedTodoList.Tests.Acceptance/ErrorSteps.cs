using TechTalk.SpecFlow;

namespace EventSourcedTodoList.Tests.Acceptance;

[Binding]
public class ErrorSteps
{
    private readonly TestApplication _application;

    public ErrorSteps(TestApplication application)
    {
        _application = application;
    }

    [Then(@"an error occurred with the message ""(.*)""")]
    public void ThenAnErrorOccurredWithTheMessage(string errorMessage)
    {
        _application.AssertErrorOccurredWithMessage(errorMessage);
    }
}