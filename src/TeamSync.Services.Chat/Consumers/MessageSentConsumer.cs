using MassTransit;
using Microsoft.Extensions.Logging;
using TeamSync.Contracts.Events;

namespace TeamSync.Services.Chat.Consumers;

public class MessageSentConsumer : IConsumer<MessageSent>
{
    private readonly ILogger<MessageSentConsumer> _logger;

    public MessageSentConsumer(ILogger<MessageSentConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MessageSent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "MessageSent event consumed: MessageId={MessageId}, ChannelId={ChannelId}, Sender={SenderDisplayName}, Content length={ContentLength}",
            message.MessageId,
            message.ChannelId,
            message.SenderDisplayName,
            message.Content.Length);

        // Placeholder for cross-service processing:
        // - Forward to notification service
        // - Update search index
        // - Trigger webhook integrations

        return Task.CompletedTask;
    }
}
