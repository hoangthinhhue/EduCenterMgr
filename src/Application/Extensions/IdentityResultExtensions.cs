// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Core.Models;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Blazor.Application.Extensions;

public static class IdentityResultExtensions
{
    public static MethodResult ToApplicationResult(this IdentityResult result)
    {
        return result.Succeeded
            ? MethodResult.Success()
            : MethodResult.Failure(result.Errors.Select(e => e.Description));
    }
}
