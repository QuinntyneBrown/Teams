using MediatR;
using Microsoft.AspNetCore.SignalR;
using TeamSync.Services.Team.Handlers;

namespace TeamSync.ApiGateway.Hubs;

public class PresenceHub : Hub
{
    private readonly IMediator _mediator;

    public PresenceHub(IMediator mediator)
    {
        _mediator = mediator;
    }

    public override async Task OnConnectedAsync()
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, "presence");
        if (Context.UserIdentifier is not null && Guid.TryParse(Context.UserIdentifier, out var userId))
        {
            var member = await _mediator.Send(new UpdatePresenceCommand(userId, "Online"));
            await Clients.Others.SendAsync("UserOnline", member);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is not null && Guid.TryParse(Context.UserIdentifier, out var userId))
        {
            var member = await _mediator.Send(new UpdatePresenceCommand(userId, "Offline"));
            await Clients.Others.SendAsync("UserOffline", member);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task UpdateStatus(string status, string? statusMessage = null)
    {
        if (Context.UserIdentifier is null || !Guid.TryParse(Context.UserIdentifier, out var userId))
            throw new HubException("Not authenticated");

        var member = await _mediator.Send(new UpdatePresenceCommand(userId, status, statusMessage));
        await Clients.All.SendAsync("UserStatusChanged", member);
    }
}
