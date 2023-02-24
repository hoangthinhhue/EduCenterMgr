using CleanArchitecture.Blazor.Infrastructure.Persistence;
using UnitMgr.API.Extensions;
using UnitMgr.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;
builder.Services
       .AddInfrastructureServices<UnitMgrDbContext>(config)
       .AddLocalizationServices()
       .AddAuthenticationService<UnitMgrDbContext>(config)
       .AddApplicationCommonServices<UnitMgrDbContext>(config)
       .AddSwaggerPage();
var app = builder.Build();
app.UseApplicationConfigure(config);
app.UseConfigureSwagger();

if (app.Environment.IsDevelopment())
{
    // Initialise and seed identity database
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
// Configure the HTTP request pipeline
await app.RunAsync();


