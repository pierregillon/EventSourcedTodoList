using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Infrastructure;

namespace TimeOnion.Configuration.HostedServices;

public static class ReplayAllDomainEventsExtensions
{
    public static async Task ReplayAllDomainEvents(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var eventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();
        var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();

        Console.WriteLine("Loading existing events ...");
        var allEvents = await eventStore.GetAll();

        Console.WriteLine($"Replaying {allEvents.Count} events ...");
        await eventPublisher.Publish(allEvents);

        Console.WriteLine("Projections are updated");
    }
}