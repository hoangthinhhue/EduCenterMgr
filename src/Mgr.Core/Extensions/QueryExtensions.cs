using Mgr.Core.Helpers;
using Mgr.Core.Interfaces;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mgr.Core.Entities;

namespace Mgr.Core.Extensions
{
    public  static class  QueryExtensions
    {
        public static Task<PaginatedData<T>> ToPageListAsync<T>(this IQueryable<T> source
                 , int pageNo = 1
                 , int pageSize = 15)
        {
            return PaginatedData<T>.CreateAsync(source, pageNo, pageSize);
        }
        public static async Task<IMethodResult<IList<T>>> ToAll<T>(this IQueryable<T> source, InputModel request)
        {
            source = QueryableHelper<T>.GetQuery(source, request.FilterParams);
            source = SortingHelper<T>.SortData(source,request.SortingParams);
            var rs =  await source.ToListAsync();
            return MethodResult<IList<T>>.ResultWithData(rs);
        }
        public static async Task<IMethodResult<IList<T>>> ToMethodReuslt<T>(this IQueryable<T> source,PaginationRequest request)
        {
            return await GetPagingFilterHelper<T>.GetDataAsync(source, request);
        }
        /// <summary>
        /// Dùng cho lấy trực tiếp không qua API
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<PaginatedData<T>> ToPageListAsync<T>(this IQueryable<T> source, PaginationRequest request)
        {
            source = QueryableHelper<T>.GetQuery(source, request.FilterParams);
            var data =  await GetPagingFilterHelper<T>.GetDataAsync(source, request);
            return await GetPagingFilterHelper<T>.GetPaginatedData(source, request);
        }
    }
}
