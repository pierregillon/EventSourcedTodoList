using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using TimeOnion.Domain.Todo;
using TimeOnion.Domain.Todo.List;

namespace TimeOnion.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ITodoListRepository, TodoListRepository>()
            .AddSingleton<IReadModelDatabase, InMemoryReadModelDatabase>()
            .AddScoped<IEventStore, S3StorageEventStore>()
            .AddScoped<MinioClient>(x =>
            {
                var configuration = x.GetRequiredService<IOptions<S3StorageConfiguration>>().Value;

                return new MinioClient()
                    .WithEndpoint(configuration.EndPoint)
                    .WithCredentials(configuration.AccessKey, configuration.SecretKey)
                    .WithRegion(configuration.Region)
                    .WithSSL()
                    .Build();
            });

        services
            .AddOptions<S3StorageConfiguration>()
            .BindConfiguration(S3StorageConfiguration.SectionName)
            .ValidateDataAnnotations();

        return services;
    }
}