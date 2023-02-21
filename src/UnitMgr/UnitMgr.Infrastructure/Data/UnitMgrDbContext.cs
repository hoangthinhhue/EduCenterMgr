using CleanArchitecture.Blazor.Domain.Common;
using Mgr.Core.Entities.Identity;
using Mgr.Core.Interface;
using System.Reflection.Metadata;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Log;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using UnitMgr.Domain.Interface.Data;

namespace UnitMgr.Infrastructure.Data;

public class UnitMgrDbContext  :IdentityDbContext
                    <ApplicationUser,ApplicationRole,Guid,ApplicationUserClaim,
                    ApplicationUserRole,ApplicationUserLogin,ApplicationRoleClaim,ApplicationUserToken>,
                    IUnitMgrDbContext
{
    public UnitMgrDbContext(
        DbContextOptions<UnitMgrDbContext> options
        ) : base(options)
    {
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Logger> Loggers { get; set; }
}
