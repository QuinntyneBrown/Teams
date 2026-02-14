using MassTransit;
using Microsoft.Extensions.Logging;
using TeamSync.Contracts.DTOs;
using TeamSync.Contracts.Events;
using TeamSync.Services.Notifications.Data;

namespace TeamSync.Services.Notifications.Consumers;

/// <summary>
/// Consumes <see cref="MeetingCreated"/> events and creates an activity feed item
/// so newly scheduled meetings appear in the dashboard "Recent Activity" section.
/// </summary>
public class MeetingCreatedActivityConsumer : IConsumer<MeetingCreated>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<MeetingCreatedActivityConsumer> _logger;

    public MeetingCreatedActivityConsumer(
        NotificationDbContext db,
        ILogger<MeetingCreatedActivityConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MeetingCreated> context)
    {
        var meeting = context.Message;

        _logger.LogInformation(
            "Processing MeetingCreated event: {Title} organized by {Organizer}",
            meeting.Title,
            meeting.OrganizerDisplayName);

        var initials = GetInitials(meeting.OrganizerDisplayName);
        var avatarColor = GenerateAvatarColor(meeting.OrganizerId);

        var activityItem = new ActivityFeedItemEntity
        {
            Id = Guid.NewGuid(),
            ActorUserId = meeting.OrganizerId,
            ActorDisplayName = meeting.OrganizerDisplayName,
            ActorInitials = initials,
            ActorAvatarColor = avatarColor,
            Action = $"scheduled a new meeting",
            Description = $"{meeting.Title} - {meeting.StartTimeUtc:MMM dd, yyyy h:mm tt} UTC",
            CreatedAt = meeting.CreatedAt,
            Category = ActivityCategory.Meeting
        };

        _db.ActivityFeedItems.Add(activityItem);

        // Also create notifications for each invited attendee
        foreach (var attendeeId in meeting.InvitedAttendeeIds)
        {
            var notification = new NotificationEntity
            {
                Id = Guid.NewGuid(),
                UserId = attendeeId,
                Title = "New Meeting Invitation",
                Description = $"{meeting.OrganizerDisplayName} invited you to \"{meeting.Title}\" on {meeting.StartTimeUtc:MMM dd, yyyy h:mm tt} UTC",
                Type = NotificationType.MeetingReminder,
                IsRead = false,
                CreatedAt = meeting.CreatedAt,
                RelatedEntityId = meeting.MeetingId
            };

            _db.Notifications.Add(notification);
        }

        await _db.SaveChangesAsync();

        _logger.LogDebug(
            "Activity feed item created and {Count} attendee notifications sent for meeting {MeetingId}",
            meeting.InvitedAttendeeIds.Count,
            meeting.MeetingId);
    }

    private static string GetInitials(string displayName)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            return "?";

        var parts = displayName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts.Length >= 2
            ? $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant()
            : parts[0][..Math.Min(2, parts[0].Length)].ToUpperInvariant();
    }

    private static string GenerateAvatarColor(Guid userId)
    {
        var colors = new[]
        {
            "#6366F1", "#8B5CF6", "#EC4899", "#EF4444",
            "#F97316", "#EAB308", "#22C55E", "#14B8A6",
            "#06B6D4", "#3B82F6"
        };

        var index = Math.Abs(userId.GetHashCode()) % colors.Length;
        return colors[index];
    }
}
