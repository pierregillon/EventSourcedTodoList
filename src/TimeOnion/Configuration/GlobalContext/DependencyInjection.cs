using MediatR;
using TimeOnion.Domain;

namespace TimeOnion.Configuration.GlobalContext;

public static class DependencyInjection
{
    public static IServiceCollection AddGlobalContextServices(this IServiceCollection services) => 
        services
        .AddScoped<InMemoryUserContextProvider>()
        .AddScoped<IUserContextProvider>(x => x.GetRequiredService<InMemoryUserContextProvider>())
        .AddTransient(typeof(IPipelineBehavior<,>), typeof(VerifyRequestAuthorization<,>));
}