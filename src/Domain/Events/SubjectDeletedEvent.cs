﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This file is part of the CleanArchitecture.Blazor project.
//     Licensed to the .NET Foundation under the MIT license.
//     See the LICENSE file in the project root for more information.
//
//     Author: neozhu
//     Created Date: 2025-02-17
//     Last Modified: 2025-02-17
//     Description: 
//       Represents a domain event that occurs when a new subject is deleted.
//       Used to signal other parts of the system that a new subject has been deleted.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CleanArchitecture.Blazor.Domain.Events;

    public class SubjectDeletedEvent : DomainEvent
    {
        public SubjectDeletedEvent(Subject item)
        {
            Item = item;
        }

        public Subject Item { get; }
    }

