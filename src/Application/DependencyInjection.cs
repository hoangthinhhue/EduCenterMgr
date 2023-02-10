// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using BlazorState;
using CleanArchitecture.Blazor.Application.Behaviours;
using CleanArchitecture.Blazor.Application.Extensions;
using CleanArchitecture.Blazor.Application.Interfaces.MultiTenant;
using CleanArchitecture.Blazor.Application.Services.MultiTenant;
using CleanArchitecture.Blazor.Application.Services.Picklist;
using CleanArchitecture.Blazor.Infrastructure.Persistence.Interceptors;
using CleanArchitecture.Core.Behaviours;
using CleanArchitecture.Core.Configurations;
using CleanArchitecture.Core.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace CleanArchitecture.Blazor.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddBlazorState((options) => options.Assemblies = new Assembly[] {
            Assembly.GetExecutingAssembly(),
        });
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(MemoryCacheBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheInvalidationBehaviour<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));
        services.AddLazyCache();
        services.AddScoped<IPicklistService, PicklistService>();
        services.AddScoped<ITenantsService, TenantsService>();
        services.AddScoped<RegisterFormModelFluentValidator>();
        return services;
    }
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseInMemoryDatabase("BlazorDashboardDb");
                options.EnableSensitiveDataLogging();
            });
        }
        else
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                      configuration.GetConnectionString("DefaultConnection"),
                      builder =>
                      {
                          builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                          builder.EnableRetryOnFailure(maxRetryCount: 5,
                                                       maxRetryDelay: TimeSpan.FromSeconds(10),
                                                       errorNumbersToAdd: null);
                          builder.CommandTimeout(15);
                      });
                options.EnableDetailedErrors(detailedErrorsEnabled: true);
                options.EnableSensitiveDataLogging();
            });
            services.AddDatabaseDeveloperPageExceptionFilter();
        }

        services.Configure<DashboardSettings>(configuration.GetSection(DashboardSettings.SectionName));
        services.Configure<AppConfigurationSettings>(configuration.GetSection(AppConfigurationSettings.SectionName));
        services.AddSingleton(s => s.GetRequiredService<IOptions<DashboardSettings>>().Value);
        services.AddScoped<IDbContextFactory<ApplicationDbContext>, BlazorContextFactory<ApplicationDbContext>>();
        services.AddTransient<ApplicationDbContext>(provider => provider.GetRequiredService<IDbContextFactory<ApplicationDbContext>>().CreateDbContext());
        services.AddScoped<ApplicationDbContextInitialiser>();

        services.AddLocalizationServices();
        services.AddServices()
                .AddSerialization()
                .AddMultiTenantService()
                .AddMessageServices(configuration)
                .AddSignalRServices();
        services.AddAuthenticationService(configuration);
        services.AddControllers();
        return services;
    }

}
