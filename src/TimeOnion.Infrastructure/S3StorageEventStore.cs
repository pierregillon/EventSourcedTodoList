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

    public async Task<IReadOnlyCollection<IDomainEvent>> GetAll()
    {
        try
        {
            await _client.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_configuration.BucketName)
                .WithObject(_configuration.ObjectName)
                .WithFile(TemporaryFilePath)
            );
        }
        catch (ObjectNotFoundException)
        {
            return Array.Empty<IDomainEvent>();
        }

        var json = await File.ReadAllTextAsync(TemporaryFilePath);

        var events = JsonSerializer.Deserialize<IEnumerable<StoredEvent>>(json)
            ?? throw new InvalidOperationException("Unable to deserialize the event list");

        return events.Select(RehydrateEvent).ToArray();
    }

    public async Task AddRange(IEnumerable<IDomainEvent> domainEvents)
    {
        var allEvents = (await GetAll()).Concat(domainEvents);

        var storedEvents = allEvents.Select(StoredEvent.From).ToArray();

        var json = JsonSerializer.Serialize(storedEvents);

        await _client.PutObjectAsync(new PutObjectArgs()
            .WithBucket(_configuration.BucketName)
            .WithObject(_configuration.ObjectName)
            .WithObjectSize(json.Length)
            .WithStreamData(new MemoryStream(Encoding.UTF8.GetBytes(json)))
        );
    }

    private IDomainEvent RehydrateEvent(StoredEvent storedEvent)
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

    public record StoredEvent(string Type, JsonElement JsonData)
    {
        public static StoredEvent From(IDomainEvent domainEvent) => new(
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
}