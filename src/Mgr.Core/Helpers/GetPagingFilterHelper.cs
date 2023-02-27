using Mgr.Core.Interfaces;
using Mgr.Core.Models;
using Mgr.Core.Entities;

namespace Mgr.Core.Helpers
{
    public static class GetPagingFilterHelper<T>
    {
        /// <summary>
        /// Lấy tất cả không dùng không phân trang
        /// </summary>
        /// <param name="query"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async static Task<IMethodResult<List<T>>> GetAll(IQueryable<T> query, InputModel request)
        {
            #region [Filter]  
            if (request != null && request.FilterParams.Any())
            {
                query = QueryableHelper<T>.GetQuery(query, request.FilterParams) ?? query;
            }
            #endregion

            #region [Sorting]  
            if (request != null && request.SortingParams != null && request.SortingParams.Any())
            {
                query = SortingHelper<T>.SortData(query, request.SortingParams) ?? query;
            }
            #endregion

            #region [Grouping]  
            if (request != null && request.GroupingColumns != null && request.GroupingColumns.Any())
            {
                query = SortingHelper<T>.GroupingData(query, request.GroupingColumns) ?? query;
            }
            #endregion

            var data = query.ToList();
            return MethodResult<List<T>>.ResultWithData(data, "Get data successfully", data.Count);
        }
        public static async Task<IMethodResult<List<T>>> GetDataAsync(IQueryable<T> query, PaginationRequest request)
        {
            #region [Filter]  
            if (request != null && request.FilterParams.Any())
            {
                query = QueryableHelper<T>.GetQuery(query, request.FilterParams) ?? query;
            }
            #endregion

            #region [Sorting]  
            if (request != null && request.SortingParams != null && request.SortingParams.Any())
            {
                query = SortingHelper<T>.SortData(query, request.SortingParams) ?? query;
            }
            #endregion

            #region [Grouping]  
            if (request != null && request.GroupingColumns != null && request.GroupingColumns.Any())
            {
                query = SortingHelper<T>.GroupingData(query, request.GroupingColumns) ?? query;
            }
            #endregion

            PaginatedData<T> data;
            if (request != null)
            {
                data = await PaginatedData<T>.CreateAsync(query, request.PageIndex, request.PageSize);
            }
            else
            {
                data = await PaginatedData<T>.CreateAsync(query, 1, 20);
            }

            return MethodResult<List<T>>.ResultWithData(data.Data, "Get data successfully", data.TotalItems);
        }
        public static async Task<PaginatedData<T>> GetPaginatedData(IQueryable<T> query, PaginationRequest request)
        {
            #region [Filter]  
            if (request != null && request.FilterParams.Any())
            {
                query = QueryableHelper<T>.GetQuery(query, request.FilterParams) ?? query;
            }
            #endregion

            #region [Sorting]  
            if (request != null && request.SortingParams != null && request.SortingParams.Any())
            {
                query = SortingHelper<T>.SortData(query, request.SortingParams) ?? query;
            }
            #endregion

            #region [Grouping]  
            if (request != null && request.GroupingColumns != null && request.GroupingColumns.Any())
            {
                query = SortingHelper<T>.GroupingData(query, request.GroupingColumns) ?? query;
            }
            #endregion

            PaginatedData<T> data;
            if (request != null)
            {
                data = await PaginatedData<T>.CreateAsync(query, request.PageIndex, request.PageSize);
            }
            else
            {
                data = await PaginatedData<T>.CreateAsync(query, 1, 20);
            }

            return data;
        }
        public static IMethodResult<List<T>> GetData(IQueryable<T> query, PaginationRequest request)
        {
            #region [Filter]  
            if (request != null && request.FilterParams.Any())
            {
                query = QueryableHelper<T>.GetQuery(query, request.FilterParams) ?? query;
            }
            #endregion

            #region [Sorting]  
            if (request != null && request.SortingParams != null && request.SortingParams.Any())
            {
                query = SortingHelper<T>.SortData(query, request.SortingParams) ?? query;
            }
            #endregion

            #region [Grouping]  
            if (request != null && request.GroupingColumns != null && request.GroupingColumns.Any())
            {
                query = SortingHelper<T>.GroupingData(query, request.GroupingColumns) ?? query;
            }
            #endregion

            PaginatedData<T> rs;
            if (request != null)
            {
                rs = PaginatedData<T>.Create(query, request.PageIndex, request.PageSize);
            }
            else
            {
                rs = PaginatedData<T>.Create(query, 1, 20);
            }

            return MethodResult<List<T>>.ResultWithData(rs.Data, "Get data successfully", rs.TotalItems);
        }
    }
}
