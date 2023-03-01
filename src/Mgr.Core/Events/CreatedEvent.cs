// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


using Mgr.Core.Abstracts;
using Mgr.Core.Entities;
using Mgr.Core.Interface;

namespace Mgr.Core.Events;
public class CreatedEvent<T> : DomainEvent where T : IBaseEntity
{
    public CreatedEvent(T entity)
    {
        Entity = entity;
    }

    public T Entity { get; }
}
