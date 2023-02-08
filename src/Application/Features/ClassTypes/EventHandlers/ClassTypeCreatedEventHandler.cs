// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.


namespace CleanArchitecture.Blazor.Application.Features.ClassTypes.EventHandlers;

    public class ClassTypeCreatedEventHandler : INotificationHandler<CreatedEvent<ClassType>>
    {
        private readonly ILogger<ClassTypeCreatedEventHandler> _logger;

        public ClassTypeCreatedEventHandler(
            ILogger<ClassTypeCreatedEventHandler> logger
            )
        {
            _logger = logger;
        }
        public Task Handle(CreatedEvent<ClassType> notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: {DomainEvent}", notification.GetType().FullName);

            return Task.CompletedTask;
        }
    }
