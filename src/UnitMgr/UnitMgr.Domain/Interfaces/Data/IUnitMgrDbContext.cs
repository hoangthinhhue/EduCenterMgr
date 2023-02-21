using Mgr.Core.Entities;
using Mgr.Core.Entities.Log;
using Microsoft.EntityFrameworkCore;

namespace UnitMgr.Domain.Interface.Data;

public interface IUnitMgrDbContext  {
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
