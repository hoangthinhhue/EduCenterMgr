using System.Data;
using System.Linq.Expressions;
using Mgr.Core;
using EFCore.BulkExtensions;
using Mgr.Core.Interfaces.Data;
using Microsoft.EntityFrameworkCore;

namespace Mgr.Core.Abstracts;

public class DbFactory<TDataContext> : IDbFactory<TDataContext>
    where TDataContext : DbContext
{
    public DbFactory(TDataContext context)
    {
        DataContext = context;
    }
    public TDataContext DataContext { get; set; }
}
