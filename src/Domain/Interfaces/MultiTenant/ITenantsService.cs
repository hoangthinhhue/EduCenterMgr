using CleanArchitecture.Blazor.Domain.DTOs.Tenants.DTOs;

namespace CleanArchitecture.Blazor.Application.Common.Interfaces.MultiTenant;
public interface ITenantsService
{
    List<TenantDto> DataSource { get; }
    event Action? OnChange;
    Task Initialize();
    Task Refresh();
}
