// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Microsoft.AspNetCore.Identity;

namespace Mgr.Core.Entities.Identity;

public class ApplicationUserRole : IdentityUserRole<Guid>
{
    public virtual ApplicationUser User { get; set; } = default!;
    public virtual ApplicationRole Role { get; set; } = default!;

}
