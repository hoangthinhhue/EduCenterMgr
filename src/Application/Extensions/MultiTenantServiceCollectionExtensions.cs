

using CleanArchitecture.Blazor.Application.Services.MultiTenant;
using CleanArchitecture.Blazor.Infrastructure.Interfaces.MultiTenant;
using CleanArchitecture.Blazor.Infrastructure.MultiTenant;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Blazor.Application.Extensions;
public static class MultiTenantServiceCollectionExtensions
{
    public static IServiceCollection AddMultiTenantService(this IServiceCollection services)
        => services.AddScoped<ITenantProvider, TenantProvider>();
}
