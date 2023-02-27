using Mgr.Core.Models;
using Microsoft.Data.SqlClient;
using System.ComponentModel;
using System.Linq.Expressions;
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
            IQueryable<T> sortQueryale = null;
            foreach (var sortingParam in sortingParams.Where(x => !String.IsNullOrEmpty(x.ColumnName)))
            {
                var col = typeof(T).GetProperty(sortingParam.ColumnName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);
                if (col != null)
                {
                    if (sortQueryale == null)
                    {
                        sortQueryale = CreataeOrderBy(data, col.Name, sortingParam.SortOrder==EnumType.SortOrders.asc.ToString()?ListSortDirection.Ascending:ListSortDirection.Descending);
                    }
                    else
                    {
                        sortQueryale = sortingParam.SortOrder == Mgr.Core.EnumType.SortOrders.asc.ToString() ? sortQueryale.OrderBy(p => 0).ThenBy(x => col.GetValue(x, null)) : sortQueryale.OrderBy(p => 0).ThenByDescending(x => col.GetValue(x, null));
                    }
                }
            }
            return sortQueryale??data;
        }
        public static IQueryable<T> CreataeOrderBy(IQueryable<T> source, string sortProperty, ListSortDirection sortOrder)
        {
            var type = typeof(T);
            var property = type.GetProperty(sortProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = sortOrder == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending";
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
