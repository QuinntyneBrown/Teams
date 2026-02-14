using MassTransit;
using Microsoft.Extensions.Logging;
using Teams.Contracts.Events;

namespace Teams.Services.Meetings.Consumers;

/// <summary>
/// Placeholder consumer that handles MeetingStarting events.
/// In a production system this would trigger push notifications, emails,
/// or SignalR alerts to meeting attendees.
/// </summary>
public class MeetingReminderConsumer : IConsumer<MeetingStarting>
{
    private readonly ILogger<MeetingReminderConsumer> _logger;

    public MeetingReminderConsumer(ILogger<MeetingReminderConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MeetingStarting> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Meeting reminder: \"{Title}\" (ID: {MeetingId}) starts in {Minutes} minutes. Notifying {AttendeeCount} attendees.",
            message.Title,
            message.MeetingId,
            message.MinutesUntilStart,
            message.AttendeeIds.Count);

        // TODO: Dispatch notifications to attendees via the Notification service
        // - Push notification through SignalR hub
        // - Email reminders for offline users
        // - Teams activity feed entry

        return Task.CompletedTask;
    }
}
