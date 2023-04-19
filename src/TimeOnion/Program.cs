using System.Reflection;
using BlazorState;
using MudBlazor.Services;
using TimeOnion.Configuration;
using TimeOnion.Domain;
using TimeOnion.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();
builder.Services.AddBlazorState
(
    options =>
        options.Assemblies =
            new[]
            {
                typeof(Program).GetTypeInfo().Assembly
            }
);
builder.Services.AddServiceHealthChecks();
builder.Services
    .AddDomain()
    .AddInfrastructure();

builder.Services.AddHostedService<SaveUncommittedEventsHostedService>();

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