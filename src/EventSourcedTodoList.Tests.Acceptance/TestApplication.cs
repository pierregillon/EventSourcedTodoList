using EventSourcedTodoList.Domain;
using EventSourcedTodoList.Domain.BuildingBlocks;
using EventSourcedTodoList.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;

namespace EventSourcedTodoList.Tests.Acceptance;

public class TestApplication
{
    private readonly ErrorDriver _errorDriver;
    private readonly ServiceProvider _serviceProvider;

    public TestApplication(ErrorDriver errorDriver)
    {
        _errorDriver = errorDriver;
        _serviceProvider = new ServiceCollection()
            .AddDomain()
            .AddInfrastructure()
            .BuildServiceProvider();
    }

    private T GetService<T>() where T : notnull
    {
        return _serviceProvider.GetRequiredService<T>();
    }

    public async Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand
    {
        var dispatcher = GetService<ICommandDispatcher>();

        await _errorDriver.TryExecute(() => dispatcher.Dispatch(command));
    }

    public async Task<TResult?> Dispatch<TResult>(IQuery<TResult> query)
    {
        var dispatcher = GetService<IQueryDispatcher>();

        return await _errorDriver.TryExecute(() => dispatcher.Dispatch(query));
    }

    public void AssertErrorOccurredWithMessage(string errorMessage)
    {
        _errorDriver.AssertErrorOccurredWithMessage(errorMessage);
    }

    public class ErrorDriver
    {
        private readonly Queue<Exception> _errors = new();
        private readonly ScenarioInfo _scenarioInfo;

        public ErrorDriver(ScenarioInfo scenarioInfo)
        {
            _scenarioInfo = scenarioInfo;
        }

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
            var lastException = _errors.Dequeue();

            Assert.NotNull(lastException);
            Assert.Equal(errorMessage, lastException.Message);
        }
    }
}