using BlazorDownloadFile;
using Blazored.LocalStorage;
using BlazorState;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using MediatR;
using Mgr.Core.Abstracts;
using Mgr.Core.Constants;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces.Data;
using Mgr.Core.Interfaces.Services;
using Mgr.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MudBlazor;
using MudBlazor.Services;
using Newtonsoft.Json;
using System.Configuration;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Uni.Core.Helper;
using UnitMgr.Admin.Services;
using UnitMgr.Admin.Services.Navigation;
using UnitMgr.Admin.Services.Notifications;
using UnitMgr.Application.Features;
using UnitMgr.Application.Features.Identity.Profile;
using UnitMgr.Application.Services.Identity;
using UnitMgr.Domain.AggregatesModel.IdentityDTOs;
using UnitMgr.Domain.Configs;
using UnitMgr.Domain.Constants;
using UnitMgr.Infrastructure.Configs.Middlewares;
using UnitMgr.Infrastructure.Data;

namespace UnitMgr.Admin.Extensions;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddBlazorUIServices(this IServiceCollection services)
    {
        services.AddRazorPages();
        services.AddServerSideBlazor(
            options =>
            {
                options.DetailedErrors = true;
                options.DisconnectedCircuitMaxRetained = 100;
                options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
                options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
                options.MaxBufferedUnacknowledgedRenderBatches = 10;
            }
            ).AddHubOptions(options =>
            {
                options.ClientTimeoutInterval = TimeSpan.FromSeconds(30);
                options.EnableDetailedErrors = false;
                options.HandshakeTimeout = TimeSpan.FromSeconds(15);
                options.KeepAliveInterval = TimeSpan.FromSeconds(15);
                options.MaximumParallelInvocationsPerClient = 1;
                options.MaximumReceiveMessageSize = 32 * 1024;
                options.StreamBufferCapacity = 10;
            })
            .AddCircuitOptions(option => { option.DetailedErrors = true; });
        services.AddMudBlazorDialog();
        services.AddHotKeys2();
        services.AddBlazoredLocalStorage();
        services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 4000;
            config.SnackbarConfiguration.HideTransitionDuration = 500;
            config.SnackbarConfiguration.ShowTransitionDuration = 500;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });
        services.AddMudExtensions();
        services.AddScoped<LayoutService>();
        services.AddBlazorDownloadFile();
        services.AddScoped<IUserPreferencesService, UserPreferencesService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<INotificationService, InMemoryNotificationService>();
        services.AddHealthChecks();
        services.AddScoped<ExceptionHandlingMiddleware>();

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddBlazorState((options) => options.Assemblies = new Assembly[] {
            Assembly.GetExecutingAssembly(),
        });

        services.AddScoped<UserProfileState>();

        services.AddValidatorsFromAssembly(Assembly.Load("UnitMgr.Domain"));

        return services;
    }

    public static IServiceCollection AddApplicationCommonServices<TDbContext>(this IServiceCollection services, IConfiguration configuration) 
        where TDbContext : DbContext
    {


        services.Scan(scan => scan.FromApplicationDependencies()
                            .AddClasses(classes => classes.AssignableTo(typeof(IBaseRepository<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
        services.Scan(scan => scan.FromApplicationDependencies()
                    .AddClasses(classes => classes.AssignableTo(typeof(IBaseService)))
                    .AsImplementedInterfaces()
                    .WithScopedLifetime());
        services.Scan(scan => scan.FromApplicationDependencies()
                            .AddClasses(classes => classes.AssignableTo(typeof(IBaseService<,,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        services.Scan(scan => scan.FromApplicationDependencies()
                            .AddClasses(classes => classes.AssignableTo(typeof(IBaseCommand<>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());

        services.Scan(scan => scan.FromApplicationDependencies()
                           .AddClasses(classes => classes.AssignableTo(typeof(IRequestHandler<,>)))
        .AsImplementedInterfaces()
        .WithScopedLifetime());
    

        return services;
    }
    public static IServiceCollection AddInfrastructureServices<TDbContext>(this IServiceCollection services, IConfiguration configuration)
           where TDbContext : DbContext
    {
        //Add configs
        services.Configure<AppConfigurationSettings>(configuration.GetSection(AppConfigurationSettings.SectionName));
        //Add automapper
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        services.AddHealthChecks();
        // Database
        if (configuration.GetValue<bool>("UseInMemoryDatabase"))
        {
            services.AddDbContext<UnitMgrDbContext>(options =>
            {
                object value = options.UseInMemoryDatabase("UnitMgrDb");
                options.EnableSensitiveDataLogging();
            });
        }
        else
        {
            //Inject DbContext
            services.AddDbContext<TDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("UnitMgrDb")));
        }
        //Inject UnitofWork
        services.AddScoped<IDbFactory<TDbContext>, DbFactory<TDbContext>>();
        services.AddScoped<IUnitOfWork<TDbContext>, UnitOfWork<TDbContext>>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        //services.AddScoped<ExceptionHandlingMiddleware>();
        services.AddLazyCache();

        //page 
        services.AddControllersWithViews();
        services.AddRazorPages();
        //Add application services
        services.AddMvc();
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly()));
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(
                builder =>
                {
                    if (configuration.GetValue<bool>("CorsAllowAnyOrigin"))
                    {
                        builder.AllowAnyOrigin();
                    }
                    else
                    {
                        var allowOrigins = configuration.GetValue<string[]>("CorsAllowOrigins");
                        if (allowOrigins != null)
                        {
                            builder.WithOrigins(allowOrigins);
                        }
                    }

                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.SetIsOriginAllowed(origin => true);
                });
        });

        services.AddScoped<IdentityDbContextInitialiser>();

        return services;

    }
    public static IServiceCollection AddLocalizationServices(this IServiceCollection services)
        => services.AddScoped<LocalizationCookiesMiddleware>()
                   .Configure<RequestLocalizationOptions>(options =>
                   {
                       options.AddSupportedUICultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
                       options.AddSupportedCultures(LocalizationConstants.SupportedLanguages.Select(x => x.Code).ToArray());
                       options.FallBackToParentUICultures = true;

                   })
                  .AddLocalization(options => options.ResourcesPath = LocalizationConstants.ResourcesPath);

    public static IServiceCollection AddAuthenticationService<TDbContext>(this IServiceCollection services, IConfiguration configuration)
        where TDbContext : DbContext
    {
        services
            .AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddDefaultTokenProviders();
        services.AddTransient<ITicketStore, InMemoryTicketStore>();
        services.AddSingleton<IPostConfigureOptions<CookieAuthenticationOptions>, ConfigureCookieAuthenticationOptions>();
        services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
            options.Lockout.MaxFailedAccessAttempts = 10;
            options.Lockout.AllowedForNewUsers = true;

            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = false;
            options.SignIn.RequireConfirmedPhoneNumber = false;

            // User settings
            options.User.RequireUniqueEmail = true;

        });
        services
                 .AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsIdentityFactory>()
                 .AddTransient<IIdentityService, IdentityService>()
                 .AddAuthorization(options =>
                 {
                     options.AddPolicy("CanPurge", policy => policy.RequireRole("Administrator"));
                     // Here I stored necessary permissions/roles in a constant
                     foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
                     {
                         var propertyValue = prop.GetValue(null);
                         if (propertyValue is not null)
                         {
                             options.AddPolicy((string)propertyValue, policy => policy.RequireClaim(ApplicationClaimTypes.Permission, (string)propertyValue));
                         }
                     }
                 })
                 .AddAuthentication()
                 .AddCookie(options =>
                 {
                     options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                     options.SlidingExpiration = true;
                     options.AccessDeniedPath = "/";
                 });

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.Configure<CookiePolicyOptions>(options =>
        {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;

        });
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = 401;
                return Task.CompletedTask;
            };
        });
        return services;
    }

    public static IServiceCollection AddSwaggerPage(this IServiceCollection services) {
        services.AddSwaggerGen(async c =>
        {
            //TODO - Lowercase Swagger Documents
            //c.DocumentFilter<LowercaseDocumentFilter>();
            //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f

            // include all project's xml comments
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (!assembly.IsDynamic)
                {
                    var xmlFile = $"{assembly.GetName().Name}.xml";
                    var xmlPath = Path.Combine(baseDirectory, xmlFile);
                    if (File.Exists(xmlPath))
                    {
                        c.IncludeXmlComments(xmlPath);
                    }
                }
            }

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = "BlazorHero.CleanArchitecture",
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
        });
        return services;
    }



}
