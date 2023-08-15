using Blazored.LocalStorage;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MudBlazor.Services;
using TimeOnion.Shared.MVU;
using TimeOnion.Shared.MVU.Pipelines;

namespace TimeOnion.Configuration.Blazor;

public static class DependencyInjection
{
    public static IServiceCollection AddBlazor(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor();
        services.AddMudServices();
        services.AddBlazoredLocalStorage();

        return services.AddMvuServices();
    }

    internal static IServiceCollection AddMvuServices(this IServiceCollection services)
    {
        services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(IStore).Assembly); });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DisplayExceptionWithSnackbar<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogExceptionPreProcessor<,>));

        services.AddScoped<IStore, InMemoryStore>();
        services.AddScoped<IActionDispatcher, MediatorActionDispatcher>();
        services.AddScoped<Subscriptions>();
        services.AddTransient<IActionEventPublisher, OnlyActiveComponentActionEventPublisher>();

        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddScoped<CustomAuthenticationStateProvider>();
        services.AddAuthorizationCore();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}