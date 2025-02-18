using Mapster;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;
using User.Events;

namespace Email.EventHandlers
{
    public class CreateUserEventHandler(ILogger<CreateUserEventHandler> logger, IBus bus) : INotificationHandler<CreateUserEvent>
    {
        public async Task Handle(CreateUserEvent notification, CancellationToken cancellationToken)
        {
            logger.LogInformation("User created notification Start: {0}", notification);

            await bus.Publish(notification.Adapt<CreatedUserSendIntegrationEvent>());

            await Task.Delay(2000, cancellationToken);

            logger.LogInformation("User created notification End: {0}", notification);
        }
    }
}