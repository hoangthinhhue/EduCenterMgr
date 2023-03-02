using EFCore.BulkExtensions;
using Mgr.Core.Entities;
using Mgr.Core.Interface;
using Mgr.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace Mgr.Core.Interfaces.Services;

public interface IBaseService
{
}
public interface IBaseService<TDataContext, T, Tkey> : IBaseService
{
    IQueryable<T> GetTable<T>(bool asNoTracking = true) where T : class;
    Task<IMethodResult<Tkey>> InsertAsync(T model);
    Task<IMethodResult<Tkey>> UpdateAsync(T model);
    Task<IMethodResult<int>> DeleteAsync(Tkey id);
    Task<IMethodResult<int>> DeleteManyAsync(List<Tkey> ids);
    Task<IMethodResult<T>> GetByIdAsync(Tkey Id);
    Task<IMethodResult<List<T>>> AllAsync();
    Task<IMethodResult<List<T>>> AllAsync(InputModel paras);
    Task<IMethodResult<List<T>>> GetData(PaginationRequest pagingParams);
    Task<IMethodResult> BulkInsertAsync(List<T> entities, BulkConfig? config = null);
    Task<IMethodResult> BulkInsertOrUpdateAsync(List<T> entities, BulkConfig? config = null);
    Task<IMethodResult> BulkInsertOrUpdateOrDeleteAsync(List<T> entities, BulkConfig? config = null);
    Task<IMethodResult> BulkUpdateAsync(List<T> entities, BulkConfig? config = null);
    Task<IMethodResult> BulkDeleteAsync(List<T> entities, BulkConfig? config = null);
    Task<IMethodResult<int>> DeleteForeverAsync(Tkey id);
    Task<IMethodResult<int>> DeleteManyForeverAsync(IList<Tkey> ids);
}