// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Entities;

namespace UnitMgr.Domain.Constants;

public static class PermissionModules
{
    public static List<string> GeneratePermissionsForModule(string module)
    {
        return new List<string>()
            {
                $"Permissions.{module}.Create",
                $"Permissions.{module}.View",
                $"Permissions.{module}.Edit",
                $"Permissions.{module}.Delete"
            };
    }
    public static List<string> GetAllPermissionsModules()
    {
        return new List<string>()
            {
                Users,
                Roles,
                Logs,
                Tenants
            };
    }
    public const string Users = "Users";
    public const string Roles = "Roles";
    public const string Logs = "Logs";
    public const string Tenants = "Tenants";
}
