using Microsoft.Extensions.DependencyInjection;
using TimeOnion.Domain.BuildingBlocks;

namespace TimeOnion.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddMediatR(x => x.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly))
            .AddScoped<ICommandDispatcher, MediatorDispatcher>()
            .AddScoped<IQueryDispatcher, MediatorDispatcher>()
            .AddScoped<IDomainEventPublisher, MediatorDispatcher>()
            ;

        return services;
    }
}