using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Teams.ApiGateway.Hubs;
using Teams.Contracts.Events;

namespace Teams.ApiGateway.Consumers;

/// <summary>
/// Bridges MassTransit MessageSent events to SignalR ChatHub clients.
/// When a message is sent (via REST or SignalR), this consumer pushes
/// the event to all connected clients in the channel.
/// </summary>
public class ChatSignalRConsumer : IConsumer<MessageSent>
{
    private readonly IHubContext<ChatHub> _chatHub;

    public ChatSignalRConsumer(IHubContext<ChatHub> chatHub)
    {
        _chatHub = chatHub;
    }

    public async Task Consume(ConsumeContext<MessageSent> context)
    {
        var msg = context.Message;
        await _chatHub.Clients.Group($"channel:{msg.ChannelId}")
            .SendAsync("ReceiveMessage", new
            {
                msg.MessageId,
                msg.ChannelId,
                msg.SenderId,
                msg.SenderDisplayName,
                msg.Content,
                msg.Timestamp
            });
    }
}

/// <summary>
/// Bridges MassTransit ChannelCreated events to all connected ChatHub clients.
/// </summary>
public class ChannelCreatedSignalRConsumer : IConsumer<ChannelCreated>
{
    private readonly IHubContext<ChatHub> _chatHub;

    public ChannelCreatedSignalRConsumer(IHubContext<ChatHub> chatHub)
    {
        _chatHub = chatHub;
    }

    public async Task Consume(ConsumeContext<ChannelCreated> context)
    {
        var evt = context.Message;
        await _chatHub.Clients.All.SendAsync("ChannelCreated", new
        {
            evt.ChannelId,
            evt.ChannelName,
            evt.Description,
            evt.CreatedByDisplayName,
            evt.CreatedAt
        });
    }
}
