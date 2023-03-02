using MessagePack;

namespace Mgr.Core.Models;

/// <summary>  
/// Filter parameters Model Class  
/// </summary>  
[MessagePackObject(keyAsPropertyName: true)]
public class FilterParams
{
    public string ColumnName { get; set; } = string.Empty;
    public string FilterValue { get; set; } = string.Empty;
    public string FilterOption { get; set; } = string.Empty;
}
