using Microsoft.Extensions.DependencyInjection;
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
            .AddSingleton<IEventStore, InMemoryEventStore>()
            ;

        return services;
    }
}