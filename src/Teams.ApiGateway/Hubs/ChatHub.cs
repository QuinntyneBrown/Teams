using MediatR;
using Microsoft.AspNetCore.SignalR;
using Teams.Services.Chat.Handlers;

namespace Teams.ApiGateway.Hubs;

public class ChatHub : Hub
{
    private readonly IMediator _mediator;

    public ChatHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Client joins a channel group to receive messages</summary>
    public async Task JoinChannel(Guid channelId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"channel:{channelId}");
        await Clients.OthersInGroup($"channel:{channelId}")
            .SendAsync("UserJoinedChannel", new { UserId = Context.UserIdentifier, ChannelId = channelId });
    }

    /// <summary>Client leaves a channel group</summary>
    public async Task LeaveChannel(Guid channelId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"channel:{channelId}");
        await Clients.OthersInGroup($"channel:{channelId}")
            .SendAsync("UserLeftChannel", new { UserId = Context.UserIdentifier, ChannelId = channelId });
    }

    /// <summary>Client sends a message to a channel</summary>
    public async Task SendMessage(Guid channelId, string content)
    {
        var userId = Guid.Parse(Context.UserIdentifier ?? throw new HubException("Not authenticated"));
        var displayName = Context.User?.FindFirst("display_name")?.Value ?? "Unknown";

        var message = await _mediator.Send(new SendMessageCommand(channelId, userId, displayName, content));

        // Broadcast to all clients in the channel
        await Clients.Group($"channel:{channelId}").SendAsync("ReceiveMessage", message);
    }

    /// <summary>Typing indicator</summary>
    public async Task StartTyping(Guid channelId)
    {
        var displayName = Context.User?.FindFirst("display_name")?.Value ?? "Unknown";
        await Clients.OthersInGroup($"channel:{channelId}")
            .SendAsync("UserTyping", new { UserId = Context.UserIdentifier, DisplayName = displayName, ChannelId = channelId });
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}
