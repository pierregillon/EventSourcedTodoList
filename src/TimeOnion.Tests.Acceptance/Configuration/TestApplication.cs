using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.GlobalContext;
using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;
using TimeOnion.Infrastructure;

namespace TimeOnion.Tests.Acceptance.Configuration;

public class TestApplication
{
    private readonly ErrorDriver _errorDriver;
    private readonly ServiceProvider _serviceProvider;

    public TestApplication(ErrorDriver errorDriver)
    {
        _errorDriver = errorDriver;

        _serviceProvider = new ServiceCollection()
            .AddScoped<IConfiguration>(_ => new ConfigurationBuilder().Build())
            .AddLogging()
            .AddDomain()
            .AddInfrastructure()
            .AddCustomAuthentication()
            .AddGlobalContextServices()
            .AddSingleton<IEventStore, InMemoryEventStore>()
            .AddSingleton(GetClockSubstitute())
            .BuildServiceProvider();
    }

    public T GetService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

    public async Task Dispatch<TCommand>(TCommand command) where TCommand : ICommand
    {
        var dispatcher = GetService<ICommandDispatcher>();

        await _errorDriver.TryExecute(() => dispatcher.Dispatch(command));
    }

    public async Task Dispatch<TCommand>(Func<TCommand> commandBuilder) where TCommand : ICommand
    {
        var dispatcher = GetService<ICommandDispatcher>();

        await _errorDriver.TryExecute(() => dispatcher.Dispatch(commandBuilder()));
    }

    public async Task<TResult?> Dispatch<TResult>(Func<ICommand<TResult>> query)
    {
        var dispatcher = GetService<ICommandDispatcher>();

        return await _errorDriver.TryExecute(() => dispatcher.Dispatch(query()));
    }

    public async Task<TResult?> Dispatch<TResult>(IQuery<TResult> query)
    {
        var dispatcher = GetService<IQueryDispatcher>();

        return await _errorDriver.TryExecute(() => dispatcher.Dispatch(query));
    }

    private static IClock GetClockSubstitute()
    {
        var clock = Substitute.For<IClock>();
        clock.Now().Returns(new SystemClock().Now());
        return clock;
    }
}