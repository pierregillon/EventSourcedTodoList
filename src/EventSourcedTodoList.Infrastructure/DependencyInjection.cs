using EventSourcedTodoList.Domain.Todo;
using EventSourcedTodoList.Domain.Todo.List;
using Microsoft.Extensions.DependencyInjection;

namespace EventSourcedTodoList.Infrastructure;

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