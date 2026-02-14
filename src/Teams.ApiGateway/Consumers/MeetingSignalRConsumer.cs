using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Teams.ApiGateway.Hubs;
using Teams.Contracts.Events;

namespace Teams.ApiGateway.Consumers;

/// <summary>
/// Bridges MassTransit meeting events to SignalR MeetingHub clients.
/// </summary>
public class MeetingCreatedSignalRConsumer : IConsumer<MeetingCreated>
{
    private readonly IHubContext<MeetingHub> _meetingHub;

    public MeetingCreatedSignalRConsumer(IHubContext<MeetingHub> meetingHub)
    {
        _meetingHub = meetingHub;
    }

    public async Task Consume(ConsumeContext<MeetingCreated> context)
    {
        var evt = context.Message;
        // Notify each invited attendee
        foreach (var attendeeId in evt.InvitedAttendeeIds)
        {
            await _meetingHub.Clients.Group($"schedule:{attendeeId}")
                .SendAsync("MeetingCreated", new
                {
                    evt.MeetingId,
                    evt.Title,
                    evt.StartTimeUtc,
                    evt.EndTimeUtc,
                    evt.OrganizerDisplayName
                });
        }
    }
}

public class MeetingStartingSignalRConsumer : IConsumer<MeetingStarting>
{
    private readonly IHubContext<MeetingHub> _meetingHub;
    private readonly IHubContext<NotificationHub> _notificationHub;

    public MeetingStartingSignalRConsumer(IHubContext<MeetingHub> meetingHub, IHubContext<NotificationHub> notificationHub)
    {
        _meetingHub = meetingHub;
        _notificationHub = notificationHub;
    }

    public async Task Consume(ConsumeContext<MeetingStarting> context)
    {
        var evt = context.Message;
        foreach (var attendeeId in evt.AttendeeIds)
        {
            await _notificationHub.Clients.Group($"user:{attendeeId}")
                .SendAsync("MeetingReminder", new
                {
                    evt.MeetingId,
                    evt.Title,
                    evt.StartTimeUtc,
                    evt.MinutesUntilStart,
                    evt.MeetingLink
                });
        }
    }
}
