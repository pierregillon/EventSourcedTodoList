using Microsoft.Extensions.Options;
using TimeOnion.Infrastructure;

namespace TimeOnion.Configuration.HostedServices;

public class CommitterHostedService : TimedHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly CommitterHostedServiceConfiguration _options;

    public CommitterHostedService(
        ILogger<TimedHostedService> logger,
        IServiceProvider serviceProvider,
        IOptions<HostedServicesConfiguration> options
    ) :
        base(logger)
    {
        _serviceProvider = serviceProvider;
        _options = options.Value.Committer;
    }

    protected override TimeSpan FromSeconds => TimeSpan.FromSeconds(_options.IntervalInSeconds!.Value);

    protected override async Task DoWorkInternal()
    {
        using var scope = _serviceProvider.CreateScope();

        var eventStore = (CachedEventStore)scope.ServiceProvider.GetRequiredService<IEventStore>();

        await eventStore.SaveUncommittedEvents();
    }
}