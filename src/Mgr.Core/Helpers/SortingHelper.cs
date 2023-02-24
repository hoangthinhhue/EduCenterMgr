using Mgr.Core.Models;
using Microsoft.Data.SqlClient;
using System.Reflection;

namespace Mgr.Core.Helpers
{
    /// <summary>  
    /// Enum for Sorting order  
    /// Asc = Ascending  
    /// Desc = Descending  
    /// </summary>  
    public static class SortingHelper<T>
    {
        /// <summary>  
        /// Actual grouping will be done in ui,   
        /// from api we will send sorted data based on grouping columns  
        /// </summary>  
        /// <param name="data"></param>  
        /// <param name="groupingColumns"></param>  
        /// <returns></returns>  
        public static IQueryable<T> GroupingData(IQueryable<T> data, IEnumerable<string> groupingColumns)
        {
            IOrderedQueryable<T>? groupedData = null;

            foreach (string grpCol in groupingColumns.Where(x => !String.IsNullOrEmpty(x)))
            {
                var col = typeof(T).GetProperty(grpCol, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    groupedData = groupedData == null ? data.OrderBy(x => col.GetValue(x, null))
                                                    : groupedData.ThenBy(x => col.GetValue(x, null));
                }
            }
            return groupedData ?? data;
        }

        /// <summary>  
        /// Actual sorting will be done in ui,   
        /// from api we will send sorted data based on grouping columns  
        /// </summary>  
        /// <param name="data"></param>  
        /// <param name="groupingColumns"></param>  
        /// <returns></returns>  
        public static IQueryable<T> SortData(IQueryable<T> data, IEnumerable<SortingParams> sortingParams)
        {
            IOrderedQueryable<T> sortedData = null;
            foreach (var sortingParam in sortingParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)))
            {
                var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    if (sortedData == null)
                    {
                        sortedData = sortingParam.SortOrder == Mgr.Core.EnumType.SortOrders.asc.ToString() ? data.OrderBy(x => col.GetValue(x, null)) : data.OrderByDescending(x => col.GetValue(x, null));
                    }
                    else
                    {
                        sortedData = sortingParam.SortOrder == Mgr.Core.EnumType.SortOrders.asc.ToString() ? sortedData.ThenBy(x => col.GetValue(x, null)) : sortedData.ThenByDescending(x => col.GetValue(x, null));
                    }
                }
            }
            return sortedData!=null?sortedData.AsQueryable() : data;
        }
    }
}
