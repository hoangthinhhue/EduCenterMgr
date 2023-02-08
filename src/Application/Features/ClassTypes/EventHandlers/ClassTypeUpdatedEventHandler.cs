// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.EventHandlers;

    public class ClassTypeUpdatedEventHandler : INotificationHandler<UpdatedEvent<ClassType>>
    {
        private readonly ILogger<ClassTypeUpdatedEventHandler> _logger;

        public ClassTypeUpdatedEventHandler(
            ILogger<ClassTypeUpdatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(UpdatedEvent<ClassType> notification, CancellationToken cancellationToken)
        {

        _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);

        return Task.CompletedTask;
        }
    }
