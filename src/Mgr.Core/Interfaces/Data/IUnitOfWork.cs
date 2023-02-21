using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Mgr.Core.Interfaces.Data;

public interface IUnitOfWork<T> : IDisposable
    where T : DbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    T CreateDbContext();
    Task<IDbContextTransaction> CreateTransactionAsync();
    IDbContextTransaction CreateTransaction();
}
