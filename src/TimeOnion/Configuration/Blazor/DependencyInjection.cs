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

        services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(IStore).Assembly); });
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(DisplayExceptionWithSnackbar<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogActionsPreProcessor<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogExceptionPreProcessor<,>));

        services.AddSingleton<IStore, InMemoryStore>();
        services.AddSingleton<IActionDispatcher, MediatorActionDispatcher>();
        services.AddSingleton<Subscriptions>();

        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddScoped<CustomAuthenticationStateProvider>();
        services.AddAuthorizationCore();

        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        return services;
    }
}