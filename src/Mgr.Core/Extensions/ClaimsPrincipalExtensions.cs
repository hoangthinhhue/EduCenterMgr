// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Constants;
using System.Security.Claims;

namespace Mgr.Core.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetEmail(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ClaimTypes.Email)?.Value.ToString() ?? "";


    public static string GetPhoneNumber(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ApplicationClaimTypes.PhoneNumber)?.Value.ToString() ?? "";

    public static string GetUserId(this ClaimsPrincipal claimsPrincipal)
       => claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value.ToString() ?? "";
    public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
       => claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value.ToString() ?? "";
    public static string GetProvider(this ClaimsPrincipal claimsPrincipal)
      => claimsPrincipal.FindFirst(ApplicationClaimTypes.Provider)?.Value.ToString() ?? "";
    public static string GetDisplayName(this ClaimsPrincipal claimsPrincipal)
         => claimsPrincipal.FindFirst(ClaimTypes.GivenName)?.Value.ToString() ?? "";
    public static string GetProfilePictureDataUrl(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ApplicationClaimTypes.ProfilePictureDataUrl)?.Value.ToString() ?? "";
    public static string GetTenantName(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ApplicationClaimTypes.TenantName)?.Value.ToString() ?? "";
    public static string GetTenantId(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ApplicationClaimTypes.TenantId)?.Value.ToString() ?? "";
    public static bool GetStatus(this ClaimsPrincipal claimsPrincipal)
       => Convert.ToBoolean(claimsPrincipal.FindFirst(ApplicationClaimTypes.Status));
    public static string GetAssignRoles(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.FindFirst(ApplicationClaimTypes.AssignRoles)?.Value.ToString() ?? "";
    public static string[] GetRoles(this ClaimsPrincipal claimsPrincipal)
        => claimsPrincipal.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).ToArray();
}

