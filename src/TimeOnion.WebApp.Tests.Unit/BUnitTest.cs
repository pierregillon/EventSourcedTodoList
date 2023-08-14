using Blazored.LocalStorage;
using Bunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using MudBlazor.Services;
using NSubstitute;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Domain.BuildingBlocks;
using TimeOnion.Domain.Todo.Core;

namespace TimeOnion.WebApp.Tests.Unit;

public abstract class BUnitTest : TestContext
{
    protected BUnitTest()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;

        Services
            .AddMvuServices()
            .AddCustomAuthentication()
            .AddSingleton<IClock>(_ => Substitute.For<IClock>())
            .AddScoped<ICommandDispatcher>(_ => Substitute.For<ICommandDispatcher>())
            .AddScoped<IQueryDispatcher>(_ => Substitute.For<IQueryDispatcher>())
            .AddScoped<ILocalStorageService>(_ => Substitute.For<ILocalStorageService>())
            .AddSingleton<IWebHostEnvironment>(_ => Substitute.For<IWebHostEnvironment>())
            .AddSingleton<IHttpContextAccessor>(_ => Substitute.For<IHttpContextAccessor>())
            .AddMudServices(config =>
            {
                config.SnackbarConfiguration.PreventDuplicates = true;
                config.SnackbarConfiguration.NewestOnTop = false;
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.TopCenter;
                config.PopoverOptions.ThrowOnDuplicateProvider = true;
            })
            .AddSingleton<IConfiguration>(_ => new ConfigurationBuilder().Build())
            .AddSingleton<MudPopoverProvider>()
            ;
        ;
    }

    protected T GetService<T>() where T : notnull => Services.GetRequiredService<T>();
}