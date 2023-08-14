using TimeOnion.Configuration;
using TimeOnion.Configuration.Authentication;
using TimeOnion.Configuration.Blazor;
using TimeOnion.Configuration.GlobalContext;
using TimeOnion.Configuration.HostedServices;
using TimeOnion.Domain;
using TimeOnion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseStaticWebAssets();

builder.Services.AddCustomAuthentication();

builder.Services.AddServiceHealthChecks();

builder.Services
    .AddDomain()
    .AddInfrastructure()
    .AddBlazor()
    .AddGlobalContextServices();

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