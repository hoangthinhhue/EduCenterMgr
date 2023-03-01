using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mgr.Core.Interface;
public interface IBaseCommand<TDataContext,T,Tkey>
    where TDataContext : DbContext
    where T : IBaseEntity<Tkey>
    where Tkey : struct
{
    IQueryable<TTable> GetTable<TTable>(bool asNoTracking = true) where TTable : class;
}