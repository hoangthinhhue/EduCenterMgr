// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.EventHandlers;

    public class ClassTypeDeletedEventHandler : INotificationHandler<DeletedEvent<ClassType>>
    {
        private readonly ILogger<ClassTypeDeletedEventHandler> _logger;

        public ClassTypeDeletedEventHandler(
            ILogger<ClassTypeDeletedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(DeletedEvent<ClassType> notification, CancellationToken cancellationToken)
        {
             _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);
            return Task.CompletedTask;
        }
    }
