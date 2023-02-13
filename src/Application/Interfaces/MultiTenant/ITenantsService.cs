using CleanArchitecture.Blazor.Application.Features.Tenants.DTOs;

namespace CleanArchitecture.Blazor.Application.Interfaces.MultiTenant;
public interface ITenantsService
{
    List<TenantDto> DataSource { get; }
    event Action? OnChange;
    Task Initialize();
    Task Refresh();
}
