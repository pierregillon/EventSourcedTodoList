using System.Reflection;
using BlazorState;
using EventSourcedTodoList.Domain;
using EventSourcedTodoList.Infrastructure;
using MudBlazor.Services;

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

builder.Services
    .AddDomain()
    .AddInfrastructure();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();