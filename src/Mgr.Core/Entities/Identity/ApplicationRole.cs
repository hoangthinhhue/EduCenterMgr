// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



using Microsoft.AspNetCore.Identity;

namespace Mgr.Core.Entities.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
    public string? Description { get; set; }
    public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
    public ApplicationRole() : base()
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        UserRoles = new HashSet<ApplicationUserRole>();
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
        RoleClaims = new HashSet<ApplicationRoleClaim>();
        UserRoles = new HashSet<ApplicationUserRole>();
    }
}
