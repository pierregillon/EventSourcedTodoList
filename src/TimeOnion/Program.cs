using MediatR;
using MudBlazor.Services;
using TimeOnion.Configuration;
using TimeOnion.Configuration.HostedServices;
using TimeOnion.Domain;
using TimeOnion.Infrastructure;
using TimeOnion.Shared.MVU;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

builder.Services.AddMediatR(configuration => { configuration.RegisterServicesFromAssembly(typeof(IStore).Assembly); });
builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LogExceptionPreProcessor<,>));

builder.Services.AddSingleton<IStore, InMemoryStore>();
builder.Services.AddSingleton<Subscriptions>();

builder.Services.AddServiceHealthChecks();
builder.Services
    .AddDomain()
    .AddInfrastructure();

builder.Services.AddCommitterHostedService();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseHealthChecksRoutes();
app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.ReplayAllDomainEvents();

app.Run();