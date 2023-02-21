using System.Data;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using MediatR;
using Mgr.Core.Entities;
using Mgr.Core.Extensions;
using Mgr.Core.Interface;
using Mgr.Core.Interfaces;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Mgr.Core.Abstracts;


public class BaseRepository<TDataContext, T> : IBaseRepository<TDataContext,T>
                  where TDataContext : DbContext
                  where T :class
{
    protected TDataContext _dataContext { get; set; }
    private DbSet<T> _DbSet { get; set; }
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    /// Contrustion
    /// </summary>
    /// <param name="dataContext"></param>
    public BaseRepository(IDbFactory<TDataContext> dbFact,
                      ICurrentUserService currentService)
    {
        _dataContext = dbFact.DataContext;
        _currentUserService = currentService;
    }

    protected virtual DbSet<T> DbSet
    {
        get
        {
            if (DbSet == null)
                _DbSet = _dataContext.Set<T>();

            return _DbSet;
        }
    }

    /// <summary>
    /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
    /// </summary>
    public virtual IQueryable<T> AllNoTracking => DbSet.AsNoTracking();
    public virtual IQueryable<T> All => DbSet.AsQueryable();
    public void Delete(T entity)
    {
        if (typeof(T).GetInterface(nameof(ISoftDelete)) != null)
        {
            var auditEntity = entity as IBaseEntity;
            if (auditEntity != null)
            {
                auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                auditEntity.UpdatedDate = DateTime.Now;
                auditEntity.IsDeleted = true;
                entity = (T)auditEntity;
            }
            Update(entity);
        }
        else
        {
            DbSet.Remove(entity);
        }
    }
    public void DeleteRange(IList<T> entities)
    {
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            var list = entities.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                var auditEntity = list[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.CreatedDate = DateTime.Now;
                    list[i] = (T)auditEntity;
                }
            }
            UpdateRange(list);
        }
        else
        {
            DbSet.RemoveRange(entities);
        }
    }
    public void DeleteForever(T entity)
    {
        DbSet.Remove(entity);
    }
    public void DeleteRangeForever(IList<T> entity)
    {
        DbSet.RemoveRange(entity);
    }

    public T Find(Expression<Func<T, bool>> expression)
    {
        return DbSet.FirstOrDefault(expression);
    }

    public Task<T> FindAsync(Expression<Func<T, bool>> expression)
    {
        return DbSet.FirstOrDefaultAsync(expression);
    }

    public void Insert(T entity)
    {
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            var auditEntity = entity as IAuditEntity;
            if (auditEntity != null)
            {
                auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                auditEntity.CreatedDate = DateTime.Now;
                entity = (T)auditEntity;
            }
        }
        DbSet.Add(entity);
    }

    public void InsertRange(IList<T> entities)
    {
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            var list = entities.ToList();
            for (int i = 0; i < list.Count(); i++)
            {
                var auditEntity = list[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.CreatedDate = DateTime.Now;
                    list[i] = (T)auditEntity;
                }
            }
            DbSet.AddRange(list);
        }
        else
        {
            DbSet.AddRange(entities);
        }
    }

    public IQueryable<T> List(Expression<Func<T, bool>> expression)
    {
        return DbSet.Where(expression);
    }
    public async Task<List<T>> ToListAsync(Expression<Func<T, bool>> expression)
    {
        return await DbSet.Where(expression).ToListAsync();
    }
    public void Update(T entity)
    {
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            var auditEntity = entity as IAuditEntity;
            if (auditEntity != null)
            {
                auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                auditEntity.UpdatedDate = DateTime.Now;
                entity = (T)auditEntity;
            }
        }
        DbSet.Update(entity);
    }
    public void UpdateRange(IList<T> entities)
    {

        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            for (int i = 0; i < entities.Count(); i++)
            {
                var auditEntity = entities[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.UpdatedDate = DateTime.Now;
                    entities[i] = (T)auditEntity;
                }
            }
        }
        entities.ToList().ForEach(q => Update(q));
    }

    public int ExcecuteCommand(string sql, params object[] parameters)
    {
        return _dataContext.Database.ExecuteSqlRaw(sql, parameters);
    }

    public Task<int> ExcecuteCommandAsync(string sql, params object[] parameters)
    {
        return _dataContext.Database.ExecuteSqlRawAsync(sql, parameters);
    }

    public IList<TResult> ExcecuteQuery<TResult>(string sql
        , CommandType commandType = CommandType.Text
        , params object[] parameters
        ) where TResult : new()
    {
        using var command = _dataContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        _dataContext.Database.OpenConnection();

        using var result = command.ExecuteReader(CommandBehavior.CloseConnection);
        var dtTable = new DataTable();
        dtTable.Load(result);

        return dtTable.ToObjects<TResult>();
    }

    public async Task<IList<TResult>> ExcecuteQueryAsync<TResult>(string sql
        , CommandType commandType = CommandType.Text
        , params object[] parameters
        ) where TResult : new()
    {
        using var command = _dataContext.Database.GetDbConnection().CreateCommand();
        command.CommandType = commandType;
        command.CommandText = sql;
        command.Parameters.AddRange(parameters);
        await _dataContext.Database.OpenConnectionAsync();

        using var result = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        var dtTable = new DataTable();
        dtTable.Load(result);

        return dtTable.ToObjects<TResult>();
    }
    public async Task BulkInsertAsync(IList<T> entities, BulkConfig? config = null)
    {
        if (config == null)
        {
            config = new BulkConfig();
            config.IncludeGraph = true;
            config.BulkCopyTimeout = 30;
        }
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var auditEntity = entities[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.CreatedDate = DateTime.Now;
                    entities[i] = (T)auditEntity;
                }
            }
        }
        await _dataContext.BulkInsertAsync(entities, config);
    }
    public async Task BulkInsertOrUpdateAsync(IList<T> entities, BulkConfig? config = null)
    {
        if (config == null)
        {
            config = new BulkConfig();
            config.IncludeGraph = true;
            config.BulkCopyTimeout = 30;
        }
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var auditEntity = entities[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.CreatedDate = DateTime.Now;
                    auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.UpdatedDate = DateTime.Now;
                    entities[i] = (T)auditEntity;
                }
            }
        }
        await _dataContext.BulkInsertOrUpdateAsync(entities, config);
    }
    public async Task BulkInsertOrUpdateOrDeleteAsync(IList<T> entities, BulkConfig? config = null)
    {
        if (config == null)
        {
            config = new BulkConfig();
            config.BulkCopyTimeout = 30;
        }
        await _dataContext.BulkInsertOrUpdateOrDeleteAsync(entities, config);
    }
    public async Task BulkUpdateAsync(IList<T> entities, BulkConfig? config = null)
    {
        if (config == null)
        {
            config = new BulkConfig();
            config.IncludeGraph = true;
            config.BulkCopyTimeout = 30;
        }
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                var auditEntity = entities[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.UpdatedDate = DateTime.Now;
                    entities[i] = (T)auditEntity;
                }
            }
        }
        await _dataContext.BulkUpdateAsync(entities, config);
    }
    public async Task BulkDeleteAsync(IList<T> entities, BulkConfig? config = null)
    {
        if (config == null)
        {
            config = new BulkConfig();
            config.BulkCopyTimeout = 30;
        }
        await _dataContext.BulkDeleteAsync(entities, config);
    }
    #region Async Method
    /// <summary>
    /// Update an Entity
    /// </summary>
    /// <param name="entity">Entity need to update</param>
    public Task UpdateAsync(T entity){
        Update(entity);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Update a list of Entities
    /// </summary>
    /// <param name="entities">Entities need to update</param>
    public Task UpdateRangeAsync(IList<T> entities) {
        UpdateRange(entities);
        return Task.CompletedTask;
    }

    /// <summary>
    /// Delete an Entity
    /// </summary>
    /// <param name="entity">Entity need to Delete</param>
    public Task DeleteAsync(T entity) {
        Delete(entity);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Delete a list of Entities
    /// </summary>
    /// <param name="entities">List entities need to Delete</param>
    public Task DeleteRangeAsync(IList<T> entities) {
        DeleteRange(entities);
        return Task.CompletedTask;
    }
    public Task DeleteForeverAsync(T entity) {
        _DbSet.Remove(entity);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Delete a list of Entities
    /// </summary>
    /// <param name="entity">List entities need to delete forever</param>
    public Task DeleteRangeForeverAsync(IList<T> entities) {
        _DbSet.RemoveRange(entities);
        return Task.CompletedTask;
    }
    /// <summary>
    /// Add new Entity
    /// </summary>
    /// <param name="entity">New Entity</param>
    public async Task<T> InsertAsync(T entity) {
        await _DbSet.AddAsync(entity);
        return entity;
    }

    /// <summary>
    /// Add a list of Entities
    /// </summary>
    /// <param name="entities">New Entities</param>
    public async Task<IList<T>> InsertRangeAsync(IList<T> entities) {
        if (typeof(T).GetInterface(nameof(IAuditEntity)) != null)
        {
            for (int i = 0; i < entities.Count(); i++)
            {
                var auditEntity = entities[i] as IAuditEntity;
                if (auditEntity != null)
                {
                    auditEntity.CreatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.CreatedDate = DateTime.Now;
                    auditEntity.UpdatedBy = Guid.Parse(_currentUserService.UserId);
                    auditEntity.UpdatedDate = DateTime.Now;
                    entities[i] = (T)auditEntity;
                }
            }
        }
        await DbSet.AddRangeAsync(entities);
        return entities;
    }

    /// <summary>
    /// Execute SqlCommand
    /// </summary>
    /// <param name="sql">SqlCommand</param>
    /// <param name="parameters">Sql parameters</param>
    /// <returns></returns>
    #endregion
}
