using TimeOnion.Infrastructure;

namespace TimeOnion.Configuration;

public class SaveUncommittedEventsHostedService : TimedHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public SaveUncommittedEventsHostedService(ILogger<TimedHostedService> logger, IServiceProvider serviceProvider) :
        base(logger) => _serviceProvider = serviceProvider;

    protected override TimeSpan FromSeconds => TimeSpan.FromSeconds(30);

    protected override async Task DoWorkInternal()
    {
        using var scope = _serviceProvider.CreateScope();

        var eventStore = (CachedEventStore)scope.ServiceProvider.GetRequiredService<IEventStore>();

        await eventStore.SaveUncommittedEvents();
    }
}