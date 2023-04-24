using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using TimeOnion.Domain;
using TimeOnion.Domain.Categories.Core;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ITodoListRepository, TodoListRepository>()
            .AddScoped<ICategoryRepository, CategoryRepository>()
            .AddScoped<IClock, SystemClock>()
            .AddSingleton<IReadModelDatabase, InMemoryReadModelDatabase>()
            .AddSingleton<DomainEventsCache>()
            .AddScoped<IEventStore, S3StorageEventStore>()
            .Decorate<IEventStore, CachedEventStore>()
            .AddScoped<MinioClient>(x =>
            {
                var configuration = x.GetRequiredService<IOptions<S3StorageConfiguration>>().Value;

                return new MinioClient()
                    .InitializeFrom(configuration)
                    .Build();
            });

        services
            .AddOptions<S3StorageConfiguration>()
            .BindConfiguration(S3StorageConfiguration.SectionName)
            .ValidateDataAnnotations();

        return services;
    }
}