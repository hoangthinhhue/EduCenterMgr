// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Abstracts;
using Mgr.Core.Interface;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mgr.Core.Entities;
public abstract class BaseEntity<Tkey> : BaseEntity
    where Tkey : struct
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public Tkey Id { get; set; }


    private readonly List<DomainEvent> _domainEvents = new();
}
public abstract class BaseEntity: IBaseEntity
{
    public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
    public Guid? CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public bool? IsDeleted { get; set; } = false;
    [NotMapped]
    private readonly List<DomainEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}