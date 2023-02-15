﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Mgr.Core.Abstracts;

namespace CleanArchitecture.Blazor.Domain.Events;


    public class CustomerUpdatedEvent : DomainEvent
    {
        public CustomerUpdatedEvent(Customer item)
        {
            Item = item;
        }

        public Customer Item { get; }
    }

