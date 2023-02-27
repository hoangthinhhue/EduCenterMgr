using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using UnitMgr.Admin.Areas.Identity;
using UnitMgr.Admin.Data;
using UnitMgr.Infrastructure.Data;
using Mgr.Core.Entities.Identity;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using UnitMgr.Admin.Extensions;
using CleanArchitecture.Blazor.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error)
                .MinimumLevel.Override("Serilog", LogEventLevel.Error)
                .MinimumLevel.Override("BlazorState.Store", LogEventLevel.Error)
                .MinimumLevel.Override("BlazorState.Subscriptions", LogEventLevel.Error)
                .MinimumLevel.Override("BlazorState.Pipeline.State.CloneStateBehavior", LogEventLevel.Error)
                .MinimumLevel.Override("BlazorState.Pipeline.RenderSubscriptions.RenderSubscriptionsPostProcessor", LogEventLevel.Error)
          .Enrich.FromLogContext()
          .Enrich.WithClientIp()
          .Enrich.WithClientAgent()
          .WriteTo.Console()
    );

builder.Services.AddBlazorUIServices();
builder.Services.AddInfrastructureServices<UnitMgrDbContext>(builder.Configuration)
                 .AddInfrastructureServices<UnitMgrDbContext>(builder.Configuration)
                       .AddLocalizationServices()
                       .AddAuthenticationService<UnitMgrDbContext>(builder.Configuration)
                       .AddApplicationCommonServices<UnitMgrDbContext>(builder.Configuration);
var app = builder.Build();
app.MapBlazorHub();
app.MapHealthChecks("/health");
app.UseExceptionHandler("/Error");
app.MapFallbackToPage("/_Host");
app.UseApplicationConfigure(builder.Configuration);

if (app.Environment.IsDevelopment())
{
    // Initialise and seed database
    using (var scope = app.Services.CreateScope())
    {
        var initialiser = scope.ServiceProvider.GetRequiredService<IdentityDbContextInitialiser>();
        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
await app.RunAsync();
