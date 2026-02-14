using Microsoft.AspNetCore.SignalR;

namespace Teams.ApiGateway.Hubs;

public class MeetingHub : Hub
{
    public async Task JoinMeeting(Guid meetingId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"meeting:{meetingId}");
    }

    public async Task LeaveMeeting(Guid meetingId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"meeting:{meetingId}");
    }

    /// <summary>Subscribe to all meeting updates for the current user</summary>
    public async Task SubscribeToSchedule()
    {
        if (Context.UserIdentifier is not null)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"schedule:{Context.UserIdentifier}");
    }
}
