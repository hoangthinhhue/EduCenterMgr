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
public class IdentityDbContextInitialiser
{
    private readonly ILogger<IdentityDbContextInitialiser> _logger;
    private readonly UnitMgrDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public IdentityDbContextInitialiser(ILogger<IdentityDbContextInitialiser> logger, UnitMgrDbContext context, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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
    private static IEnumerable<string> GetAllPermissions()
    {
        var allPermissions = new List<string>();
        var modules = typeof(Permissions).GetNestedTypes();

        foreach (var module in modules)
        {
            var moduleName = string.Empty;
            var moduleDescription = string.Empty;

            var fields = module.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);

            foreach (var fi in fields)
            {
                var propertyValue = fi.GetValue(null);

                if (propertyValue is not null)
                    allPermissions.Add((string)propertyValue);
            }
        }

        return allPermissions;
    }
    public async Task TrySeedAsync()
    {
        // Default tenants
        if (!_context.Tenants.Any())
        {
            _context.Tenants.Add(new Tenant() { Name = "Master", Description = "Master Site" });
            _context.Tenants.Add(new Tenant() { Name = "Slave", Description = "Slave Site" });
            await _context.SaveChangesAsync();

        }

        // Default roles
        var administratorRole = new ApplicationRole(RoleNameConstants.Administrator) { Description = "Admin Group" };
        var userRole = new ApplicationRole(RoleNameConstants.Users) { Description = "Basic Group" };
        var Permissions = GetAllPermissions();
        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
           
            foreach (var permission in Permissions)
            {
                await _roleManager.AddClaimAsync(administratorRole, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }
        if (_roleManager.Roles.All(r => r.Name != userRole.Name))
        {
            await _roleManager.CreateAsync(userRole);
            foreach (var permission in Permissions)
            {
                if (permission.StartsWith("Permissions.Products"))
                    await _roleManager.AddClaimAsync(userRole, new Claim(ApplicationClaimTypes.Permission, permission));
            }
        }
        // Default users
        var administrator = new ApplicationUser { UserName = UserNameConstants.Administrator, Provider = "Local", IsActive = true, TenantId = _context.Tenants.First().Id, TenantName = _context.Tenants.First().Name, DisplayName = "Administrator", Email = "admin@local.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://s.gravatar.com/avatar/78be68221020124c23c665ac54e07074?s=80" };
        var demo = new ApplicationUser { UserName = UserNameConstants.Demo, IsActive = true, Provider = "Local", TenantId = _context.Tenants.First().Id, TenantName = _context.Tenants.First().Name, DisplayName = "Demo", Email = "neozhu@126.com", EmailConfirmed = true, ProfilePictureDataUrl = $"https://s.gravatar.com/avatar/ea753b0b0f357a41491408307ade445e?s=80" };
        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, UserNameConstants.DefaultPassword);
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name! });
        }
        if (_userManager.Users.All(u => u.UserName != demo.UserName))
        {
            await _userManager.CreateAsync(demo, UserNameConstants.DefaultPassword);
            await _userManager.AddToRolesAsync(demo, new[] { userRole.Name! });
        }
    }
}
