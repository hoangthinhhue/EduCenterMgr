using EFCore.BulkExtensions;
using Mgr.Core.Entities;
using Mgr.Core.Interface;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace Mgr.Core.Interfaces.Data;

public interface IBaseRepository<TDataContext, T> 
    where TDataContext : DbContext
    where T: class
{
    IQueryable<T> All { get; }
    IQueryable<T> AllNoTracking { get; }
    /// <summary>
    /// Find an entity with expressionss
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>Entity</returns>
    T Find(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Find an entity with expression
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>Entity</returns>
    Task<T> FindAsync(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Find all entities with expression
    /// </summary>
    /// <param name="expression"></param>
    /// <returns>List of entities</returns>
    IQueryable<T> List(Expression<Func<T, bool>> expression);
    Task<List<T>> ToListAsync(Expression<Func<T, bool>> expression);
    #region Sync Method
    /// <summary>
    /// Update an Entity
    /// </summary>
    /// <param name="entity">Entity need to update</param>
    void Update(T entity);

    /// <summary>
    /// Update a list of Entities
    /// </summary>
    /// <param name="entities">Entities need to update</param>
    void UpdateRange(IList<T> entities);

    /// <summary>
    /// Delete an Entity
    /// </summary>
    /// <param name="entity">Entity need to Delete</param>
    void Delete(T entity);
    /// <summary>
    /// Delete an Entity forever
    /// </summary>
    /// <param name="entity"></param>
    public void DeleteForever(T entity);
    public void DeleteRangeForever(IList<T> entity);

    /// <summary>
    /// Delete a list of Entities
    /// </summary>
    /// <param name="entities">List entities need to Delete</param>
    void DeleteRange(IList<T> entities);
    /// <summary>
    /// Add new Entity
    /// </summary>
    /// <param name="entity">New Entity</param>
    void Insert(T entity);

    /// <summary>
    /// Add a list of Entities
    /// </summary>
    /// <param name="entities">New Entities</param>
    void InsertRange(IList<T> entities);

    /// <summary>
    /// Execute SqlCommand
    /// </summary>
    /// <param name="sql">SqlCommand</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    #endregion

    #region Async Method
    /// <summary>
    /// Update an Entity
    /// </summary>
    /// <param name="entity">Entity need to update</param>
    Task UpdateAsync (T entity);

    /// <summary>
    /// Update a list of Entities
    /// </summary>
    /// <param name="entities">Entities need to update</param>
    Task UpdateRangeAsync(IList<T> entities);

    /// <summary>
    /// Delete an Entity
    /// </summary>
    /// <param name="entity">Entity need to Delete</param>
    Task DeleteAsync(T entity);
    /// <summary>
    /// Delete an Entity forever
    /// </summary>
    /// <param name="entity"></param>
    Task DeleteForeverAsync(T entity);
        /// <summary>
    /// Delete a list of Entities
    /// </summary>
    /// <param name="entity">List entities need to delete forever</param>
    Task DeleteRangeForeverAsync(IList<T> entities);
    /// <summary>
    /// Delete a list of Entities
    /// </summary>
    /// <param name="entities">List entities need to Delete</param>
    Task DeleteRangeAsync(IList<T> entities);



    /// <summary>
    /// Add new Entity
    /// </summary>
    /// <param name="entity">New Entity</param>
    Task<T> InsertAsync (T entity);

    /// <summary>
    /// Add a list of Entities
    /// </summary>
    /// <param name="entities">New Entities</param>
    Task<IList<T>> InsertRangeAsync(IList<T> entities);

    /// <summary>
    /// Execute SqlCommand
    /// </summary>
    /// <param name="sql">SqlCommand</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    #endregion

    int ExcecuteCommand(string sql, params object[] parameters);

    /// <summary>
    /// Execute SqlCommand
    /// </summary>
    /// <param name="sql">SqlCommand</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    Task<int> ExcecuteCommandAsync(string sql, params object[] parameters);

    /// <summary>
    /// Execute Sql Qeury
    /// </summary>
    /// <typeparam name="TResult">Type of result returned</typeparam>
    /// <param name="sql">SqlQuery</param>
    /// <param name="commandType">SP or Text</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    IList<TResult> ExcecuteQuery<TResult>(string sql
        , CommandType commandType = CommandType.Text
        , params object[] parameters
        ) where TResult : new();

    /// <summary>
    /// Execute Sql Qeury
    /// </summary>
    /// <typeparam name="TResult">Type of result returned</typeparam>
    /// <param name="sql">SqlQuery</param>
    /// <param name="commandType">SP or Text</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    Task<IList<TResult>> ExcecuteQueryAsync<TResult>(string sql
        , CommandType commandType = CommandType.Text
        , params object[] parameters
        ) where TResult : new();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="entities"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    Task BulkInsertAsync(IList<T> entities, BulkConfig? config = null);
    Task BulkInsertOrUpdateAsync(IList<T> entities, BulkConfig? config = null);
    Task BulkInsertOrUpdateOrDeleteAsync(IList<T> entities, BulkConfig? config = null);
    Task BulkUpdateAsync(IList<T> entities, BulkConfig? config = null);
    Task BulkDeleteAsync(IList<T> entities, BulkConfig? config = null);
}
