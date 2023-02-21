using System.Data;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mgr.Core.Abstracts;

public class UnitOfWork<T> : IUnitOfWork<T>
    where T : DbContext
{
    private readonly T _dbContext;
    public UnitOfWork(IDbFactory<T> dbFactory)
    {
        _dbContext = dbFactory.DataContext;
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
    public async Task<IDbContextTransaction> CreateTransactionAsync()
    {
        return await _dbContext.Database.BeginTransactionAsync();
    }
    public IDbContextTransaction CreateTransaction()
    {
        return _dbContext.Database.BeginTransaction();
    }

    public T CreateDbContext()
    {
        return _dbContext;
    }
}
