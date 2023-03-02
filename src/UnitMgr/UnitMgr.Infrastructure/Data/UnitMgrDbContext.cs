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

public partial class UnitMgrDbContext 
{
    public UnitMgrDbContext(
        DbContextOptions<UnitMgrDbContext> options
        ) : base(options)
    {
    }
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<Logger> Loggers { get; set; }
}
