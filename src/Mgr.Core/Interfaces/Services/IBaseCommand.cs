using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Mgr.Core.Interface;
public interface IBaseCommand<TDataContext>
     where TDataContext : DbContext
{
    IQueryable<T> GetTable<T>(bool asNoTracking = true) where T : class;
}