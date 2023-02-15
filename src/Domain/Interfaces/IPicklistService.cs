using CleanArchitecture.Blazor.Domain.DTOs.KeyValues.DTOs;

namespace CleanArchitecture.Blazor.Domain.Interfaces;
public interface IPicklistService
{
    List<KeyValueDto> DataSource { get; } 
    event Action? OnChange;
    Task Initialize();
    Task Refresh();
}
