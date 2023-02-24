using MessagePack;
using Mgr.Core.EnumType;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mgr.Core.Models
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class InputModel
    {
        public InputModel()
        {
            SortingParams = new List<SortingParams>();
            FilterParams = new List<FilterParams>();
        }
        public IList<SortingParams> SortingParams { set; get; }
        public IList<FilterParams> FilterParams { get; set; }
        public IList<string> GroupingColumns { get; set; } = new List<string>();

        public T GetFilter<T>(string name, bool IsRemoved = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return default(T);
                var filter = FilterParams.FirstOrDefault(q => q.ColumnName == name.Trim());
                if (filter == null)
                    return default(T);
                T value = (T)Convert.ChangeType(filter.FilterValue, typeof(T));
                if (IsRemoved)
                    FilterParams.Remove(filter);
                return value;
            }
            catch (Exception)
            {

                return default(T);
            }
        }
        public void AddFilter<T>(string name, T value, OperationExpression filterOption = OperationExpression.equal)
        {
            FilterParams.Add(new FilterParams() { ColumnName = name, FilterValue = value.ToString(), FilterOption = filterOption.ToString()});
        }
        public void AddSort(string name, SortOrders sortOrders = SortOrders.asc)
        {
            SortingParams.Add(new SortingParams { ColumnName = name, SortOrder = sortOrders.ToString() });
        }
    }
}