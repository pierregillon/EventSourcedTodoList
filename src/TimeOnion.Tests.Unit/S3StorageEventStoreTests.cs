using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.List;
using TimeOnion.Infrastructure;

namespace TimeOnion.Tests.Unit;

public class S3StorageEventStoreTests
{
    private readonly S3StorageConfiguration _configuration;
    private readonly S3StorageEventStore _eventStore;
    private readonly MinioClient _minio;

    public S3StorageEventStoreTests()
    {
        var serviceProvider = new ServiceCollection()
            .AddInfrastructure()
            .AddScoped<IConfiguration>(_ => new ConfigurationManager().AddInMemoryCollection().Build())
            .Configure<S3StorageConfiguration>(configuration =>
            {
                configuration.EndPoint = "s3.fr-par.scw.cloud";
                configuration.UseSSL = true;
                configuration.Region = "fr-par";
                configuration.AccessKey = "";
                configuration.SecretKey = "";
                configuration.BucketName = "time-onion-bucket";
                configuration.ObjectName = Guid.NewGuid().ToString();
            })
            .BuildServiceProvider();

        _minio = serviceProvider.GetRequiredService<MinioClient>();
        _eventStore = ActivatorUtilities.CreateInstance<S3StorageEventStore>(serviceProvider);
        _configuration = serviceProvider.GetRequiredService<IOptions<S3StorageConfiguration>>().Value;
    }

    [Fact(Skip = "infra")]
    public async Task Unknown_object_does_not_throw_error_on_getting_events()
    {
        var domainEvents = await _eventStore.GetAll();

        domainEvents.Should().BeEmpty();
    }

    [Fact(Skip = "infra")]
    public async Task Adding_domain_events_on_unknown_object_name_creates_it()
    {
        try
        {
            await _eventStore.Save(new IDomainEvent[]
            {
                new TodoItemAdded(TodoListId.New(), TodoItemId.New(), new ItemDescription("test"), Temporality.ThisDay)
            });

            var stat = await _minio.StatObjectAsync(new StatObjectArgs().InitializeFrom(_configuration));

            stat.Should().NotBeNull();
        }
        finally
        {
            await _minio.RemoveObjectAsync(new RemoveObjectArgs().InitializeFrom(_configuration));
        }
    }

    [Fact(Skip = "infra")]
    public async Task Added_domain_events_are_correctly_retrieved()
    {
        try
        {
            var todoItemAdded = new TodoItemAdded(TodoListId.New(), TodoItemId.New(), new ItemDescription("test"),
                Temporality.ThisWeek);

            await _eventStore.Save(new IDomainEvent[]
            {
                todoItemAdded
            });

            var domainEvents = await _eventStore.GetAll();

            domainEvents
                .Should()
                .ContainEquivalentOf(todoItemAdded)
                .And.HaveCount(1);
        }
        finally
        {
            await _minio.RemoveObjectAsync(new RemoveObjectArgs().InitializeFrom(_configuration));
        }
    }
}