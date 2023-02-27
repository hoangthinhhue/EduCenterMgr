using System.Reflection;
using System.Security.Claims;
using Mgr.Core.Constants;
using Mgr.Core.Entities;
using Mgr.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnitMgr.Domain.Constants;
using UnitMgr.Infrastructure.Data;

namespace CleanArchitecture.Blazor.Infrastructure.Persistence;
public class UnitMgrDbContextInitialiser
{
    private readonly ILogger<UnitMgrDbContextInitialiser> _logger;
    private readonly UnitMgrDbContext _context;
    public UnitMgrDbContextInitialiser(ILogger<UnitMgrDbContextInitialiser> logger, UnitMgrDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
    }
    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }
    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
            _context.ChangeTracker.Clear();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
    public async Task TrySeedAsync()
    {
        //TO_DO Seed Data
        await Task.CompletedTask;
    }
}
