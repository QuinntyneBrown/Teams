using Microsoft.AspNetCore.SignalR;

namespace TeamSync.ApiGateway.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        if (Context.UserIdentifier is not null)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user:{Context.UserIdentifier}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (Context.UserIdentifier is not null)
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user:{Context.UserIdentifier}");
        await base.OnDisconnectedAsync(exception);
    }
}
