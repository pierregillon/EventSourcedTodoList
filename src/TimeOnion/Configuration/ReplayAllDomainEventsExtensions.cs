using System.Text.Json;
using Microsoft.Extensions.Options;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Infrastructure;

namespace TimeOnion.Configuration;

public static class ReplayAllDomainEventsExtensions
{
    public static async Task ReplayAllDomainEvents(this WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();
        var eventPublisher = scope.ServiceProvider.GetRequiredService<IDomainEventPublisher>();
        var eventStore = scope.ServiceProvider.GetRequiredService<IEventStore>();
        var configuration = scope.ServiceProvider.GetRequiredService<IOptions<S3StorageConfiguration>>();

        Console.WriteLine(JsonSerializer.Serialize(configuration));
        
        logger.LogInformation("Loading existing events ...");
        var allEvents = await eventStore.GetAll();

        logger.LogInformation($"Replaying {allEvents.Count} events ...");
        await eventPublisher.Publish(allEvents);

        logger.LogInformation("Projection are updated");
    }
}