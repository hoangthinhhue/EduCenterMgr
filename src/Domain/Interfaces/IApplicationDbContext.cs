// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.



using CleanArchitecture.Blazor.Domain.Entities.Audit;
using CleanArchitecture.Blazor.Domain.Entities.Log;
using CleanArchitecture.Blazor.Domain.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace CleanArchitecture.Blazor.Domain.Interfaces;

public interface IApplicationDbContext 
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    DbSet<Logger> Loggers { get; set; }
    DbSet<AuditTrail> AuditTrails { get; set; }
    DbSet<Document> Documents { get; set; }
    DbSet<KeyValue> KeyValues { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Tenant> Tenants { get; set; }
    DbSet<Customer> Customers { get; set; }
    DbSet<ClassType> ClassTypes { get; set; }
    ChangeTracker ChangeTracker { get; }
}
