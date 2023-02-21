using MessagePack;
using Mgr.Core.EnumType;

namespace Mgr.Core.Models
{
    /// <summary>  
    /// Sorting parameters Model Class  
    /// </summary>  
    [MessagePackObject(keyAsPropertyName: true)]
    public class SortingParams
    {
       /// <summary>
       /// Asc, Desc
       /// </summary>
        public string SortOrder { get; set; } = SortOrders.Asc.ToString();
        public string? ColumnName { get; set; }
    }
}
