// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CleanArchitecture.Domain.Interfaces.Mappings;

namespace CleanArchitecture.Blazor.Domain.DTOs.Tenants.DTOs;


public class TenantDto:IMapFrom<Tenant>
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? Name { get; set; }
    public string? Description { get; set; }

}

