﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This file is part of the CleanArchitecture.Blazor project.
//     Licensed to the .NET Foundation under one or more agreements.
//     The .NET Foundation licenses this file to you under the MIT license.
//     See the LICENSE file in the project root for more information.
//
//     Author: neozhu
//     Created Date: 2025-02-17
//     Last Modified: 2025-02-17
//     Description: 
//       Handles the `SubjectDeletedEvent` which occurs when a new subject is deleted.
//       This event handler can be extended to trigger additional actions, such as 
//       sending notifications or updating other systems.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CleanArchitecture.Blazor.Application.Features.Subjects.EventHandlers;

    public class SubjectDeletedEventHandler : INotificationHandler<SubjectDeletedEvent>
    {
        private readonly ILogger<SubjectDeletedEventHandler> _logger;

        public SubjectDeletedEventHandler(
            ILogger<SubjectDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(SubjectDeletedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handled domain event '{EventType}' with notification: {@Notification} ", notification.GetType().Name, notification);
            return Task.CompletedTask;
        }
    }
