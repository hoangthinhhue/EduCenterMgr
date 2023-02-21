// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;

namespace Mgr.Core.Entities.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<Guid>
{
    public string? Description { get; set; }
    public string? Group { get; set; }
    public virtual ApplicationRole Role { get; set; } = default!;

    
}
