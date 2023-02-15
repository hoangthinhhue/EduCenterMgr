// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Entities;
using Mgr.Core.Models;

namespace CleanArchitecture.Blazor.Infrastructure.Extensions;

public static class IdentityResultExtensions
{
    public static MethodResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? MethodResult.ResultWithSuccess()
            : MethodResult.ResultWithError(result.Errors.Select(e => e.Description).ToString());
    }
}
