using MassTransit;
using Microsoft.Extensions.Logging;
using Shared.Messaging.Events;

namespace Email.EventHandlers
{
    public class CreatedUserSendIntegrationEventHandler(ILogger<CreatedUserSendIntegrationEventHandler> logger) : IConsumer<CreatedUserSendIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<CreatedUserSendIntegrationEvent> context)
        {
            logger.LogInformation("User created Send Email event Start: {0}", context.Message);

            await Task.Delay(2000);

            logger.LogInformation("User created Send Email event End: {0}", context.Message);
        }
    }
}