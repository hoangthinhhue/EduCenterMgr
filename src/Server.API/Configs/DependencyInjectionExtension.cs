using CleanArchitecture.Blazor.Application.Common.Interfaces;
using CleanArchitecture.Blazor.Application.Services;
using CleanArchitecture.Blazor.Domain.Identity;
using CleanArchitecture.Blazor.Infrastructure.Configurations;
using CleanArchitecture.Blazor.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Configuration;
using System.Text.Json.Serialization;
using CleanArchitecture.Blazor.Infrastructure.Constants.Database;
using CleanArchitecture.Blazor.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CleanArchitecture.Blazor.Infrastructure.Constants.Localization;
using CleanArchitecture.Blazor.Server.API.Middlewares;
using CleanArchitecture.Blazor.Application.Common.Interfaces.Identity;
using CleanArchitecture.Blazor.Infrastructure.Services.Identity;
using Microsoft.EntityFrameworkCore;
using CleanArchitecture.Blazor.Infrastructure.Services.MultiTenant;

namespace Server.API.Configs;

public static class DependencyInjectionExtension
{
    private const int DEFAULT_LOCKOUT_TIME_SPAN_MINUTES = 5;
    private const int MAX_FAILED_ACCESS_ATTEMPTS = 5;

    public static IServiceCollection AddAPIService(this IServiceCollection services, IConfiguration configuration)
    {


        // Configure DbContext with SQL Server
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetValue<string>("DatabaseSettings:ConnectionString")));

        services.AddScoped<IUserStore<ApplicationUser>, MultiTenantUserStore>();
        services.AddScoped<UserManager<ApplicationUser>, MultiTenantUserManager>();
        services.AddIdentityCore<ApplicationUser>()
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddClaimsPrincipalFactory<MultiTenantUserClaimsPrincipalFactory>()
            .AddDefaultTokenProviders();

        // Find the default RoleValidator<ApplicationRole> registration in the service collection.
        var defaultRoleValidator = services.FirstOrDefault(descriptor => descriptor.ImplementationType == typeof(RoleValidator<ApplicationRole>));

        // If the default role validator is found, remove it to ensure only MultiTenantRoleValidator is used.
        if (defaultRoleValidator != null)
        {
            services.Remove(defaultRoleValidator);
        }
        services.Configure<IdentityOptions>(options =>
        {
            var identitySettings = configuration.GetRequiredSection("IdentitySettings").Get<IdentitySettings>();
            identitySettings = identitySettings ?? new IdentitySettings();
            // Password settings
            options.Password.RequireDigit = identitySettings.RequireDigit;
            options.Password.RequiredLength = identitySettings.RequiredLength;
            options.Password.RequireNonAlphanumeric = identitySettings.RequireNonAlphanumeric;
            options.Password.RequireUppercase = identitySettings.RequireUpperCase;
            options.Password.RequireLowercase = identitySettings.RequireLowerCase;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(DEFAULT_LOCKOUT_TIME_SPAN_MINUTES);
            options.Lockout.MaxFailedAccessAttempts = MAX_FAILED_ACCESS_ATTEMPTS;
            options.Lockout.AllowedForNewUsers = true;

            // Default SignIn settings.
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedPhoneNumber = false;
            options.SignIn.RequireConfirmedAccount = true;

            // User settings
            options.User.RequireUniqueEmail = true;
            //options.Tokens.EmailConfirmationTokenProvider = "Email";

        });

        // Configure CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader());
        });


        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo { Title = "Test API", Version = "v1" });
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
        });
        });

        services.AddProblemDetails();
        services.AddRouting(options => options.LowercaseUrls = true);

        // Register our TokenService dependency
        services.AddScoped<ITokenService, TokenService>();

        // Support string to enum conversions
        services.AddControllers().AddJsonOptions(opt =>
        {
            opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });



        // These will eventually be moved to a secrets file, but for alpha development appsettings is fine
        var validIssuer = configuration.GetValue<string>("JwtTokenSettings:ValidIssuer");
        var validAudience = configuration.GetValue<string>("JwtTokenSettings:ValidAudience");
        var symmetricSecurityKey = configuration.GetValue<string>("JwtTokenSettings:SymmetricSecurityKey");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(options =>
            {
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ClockSkew = TimeSpan.Zero,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = validIssuer,
                    ValidAudience = validAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(symmetricSecurityKey)
                    ),
                };
            });
        return services;
    }
}
