using Microsoft.Extensions.DependencyInjection;
using TimeOnion.Domain;
using TimeOnion.Domain.BuildingBlocks;
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
            .AddDomain()
            .AddInfrastructure()
            .BuildServiceProvider();
    }

    private T GetService<T>() where T : notnull => _serviceProvider.GetRequiredService<T>();

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
}