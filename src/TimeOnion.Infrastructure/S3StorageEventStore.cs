using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;
using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Infrastructure;

public class S3StorageEventStore : IEventStore
{
    private static readonly string TemporaryFilePath = Path.GetTempFileName();

    private readonly MinioClient _client;
    private readonly S3StorageConfiguration _configuration;

    public S3StorageEventStore(MinioClient client, IOptions<S3StorageConfiguration> configuration)
    {
        _client = client;
        _configuration = configuration.Value;
    }

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll(Guid aggregateId) => (await GetAllStoredEvents())
        .Where(x => x.AggregateId == aggregateId)
        .Select(RehydrateEvent)
        .ToArray();

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll() => (await GetAllStoredEvents())
        .Select(RehydrateEvent)
        .ToArray();

    public async Task Save(IEnumerable<IDomainEvent> domainEvents)
    {
        var storedEvents = domainEvents
            .Select(StoredEvent.From)
            .ToArray();

        var allEvents = (await GetAllStoredEvents()).Concat(storedEvents).ToArray();

        var json = JsonSerializer.Serialize(allEvents);

        var args = new PutObjectArgs()
            .InitializeFrom(_configuration)
            .WithObjectSize(json.Length)
            .WithStreamData(new MemoryStream(Encoding.UTF8.GetBytes(json)));

        await _client.PutObjectAsync(args);
    }

    private async Task<IReadOnlyCollection<StoredEvent>> GetAllStoredEvents()
    {
        try
        {
            var args = new GetObjectArgs()
                .InitializeFrom(_configuration)
                .WithFile(TemporaryFilePath);

            await _client.GetObjectAsync(args);
        }
        catch (ObjectNotFoundException)
        {
            return Array.Empty<StoredEvent>();
        }

        var json = await File.ReadAllTextAsync(TemporaryFilePath);

        return JsonSerializer.Deserialize<IReadOnlyCollection<StoredEvent>>(json)
            ?? throw new InvalidOperationException("Unable to deserialize the event list");
    }

    private static IDomainEvent RehydrateEvent(StoredEvent storedEvent)
    {
        var type = typeof(IDomainEvent).Assembly
            .GetTypes()
            .SingleOrDefault(x => x.Name == storedEvent.Type);

        if (type is null)
        {
            throw new InvalidOperationException($"Unable to rehydrate the event {storedEvent.Type}");
        }

        return (IDomainEvent)(storedEvent.JsonData.Deserialize(type)
            ?? throw new InvalidOperationException($"Unable to deserialize the event {storedEvent.Type}"));
    }

    public record StoredEvent(Guid AggregateId, string Type, JsonElement JsonData)
    {
        public static StoredEvent From(IDomainEvent domainEvent) => new(
            domainEvent.AggregateId,
            domainEvent.GetType().Name,
            JsonSerializer.SerializeToElement(domainEvent, domainEvent.GetType())
        );
    }
}

public class S3StorageConfiguration
{
    public const string SectionName = "S3StorageConfiguration";

    [Required] public string EndPoint { get; set; } = default!;

    [Required] public string BucketName { get; set; } = default!;

    [Required] public string ObjectName { get; set; } = default!;

    [Required] public string AccessKey { get; set; } = default!;

    [Required] public string SecretKey { get; set; } = default!;
    [Required] public string Region { get; set; } = default!;
    [Required] public bool UseSSL { get; set; } = true;
}

public static class BucketArgsExtensions
{
    public static T InitializeFrom<T>(this T args, S3StorageConfiguration configuration) where T : ObjectArgs<T> => args
        .WithBucket(configuration.BucketName)
        .WithObject(configuration.ObjectName);
}

public static class MinioExtensions
{
    public static MinioClient InitializeFrom(this MinioClient client, S3StorageConfiguration configuration) => client
        .WithEndpoint(configuration.EndPoint)
        .WithCredentials(configuration.AccessKey, configuration.SecretKey)
        .WithRegion(configuration.Region)
        .WithSSL(configuration.UseSSL);
}