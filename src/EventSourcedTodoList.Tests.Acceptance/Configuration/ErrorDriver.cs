using TechTalk.SpecFlow;

namespace EventSourcedTodoList.Tests.Acceptance;

public class ErrorDriver
{
    private readonly Queue<Exception> _errors = new();
    private readonly ScenarioInfo _scenarioInfo;

    public ErrorDriver(ScenarioInfo scenarioInfo) => _scenarioInfo = scenarioInfo;

    public async Task TryExecute(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (Exception exception)
        {
            if (!_scenarioInfo.Tags.Contains("ErrorHandling")) throw;

            _errors.Enqueue(exception);
        }
    }

    public async Task<TResult?> TryExecute<TResult>(Func<Task<TResult>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception exception)
        {
            if (!_scenarioInfo.Tags.Contains("ErrorHandling")) throw;

            _errors.Enqueue(exception);

            return default;
        }
    }

    public void AssertErrorOccurredWithMessage(string errorMessage)
    {
        Assert.NotEmpty(_errors);

        var lastException = _errors.Dequeue();

        Assert.NotNull(lastException);
        Assert.Equal(errorMessage, lastException.Message);
    }

    public void AssertNoErrorOccurred() => Assert.Empty(_errors);
}