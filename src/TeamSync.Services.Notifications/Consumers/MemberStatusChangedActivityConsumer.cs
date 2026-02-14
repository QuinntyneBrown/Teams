using MassTransit;
using Microsoft.Extensions.Logging;
using TeamSync.Contracts.DTOs;
using TeamSync.Contracts.Events;
using TeamSync.Services.Notifications.Data;

namespace TeamSync.Services.Notifications.Consumers;

/// <summary>
/// Consumes <see cref="MemberStatusChanged"/> events and creates an activity feed item
/// so status changes (online, away, in a meeting, etc.) appear in the dashboard
/// "Recent Activity" section.
/// </summary>
public class MemberStatusChangedActivityConsumer : IConsumer<MemberStatusChanged>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<MemberStatusChangedActivityConsumer> _logger;

    public MemberStatusChangedActivityConsumer(
        NotificationDbContext db,
        ILogger<MemberStatusChangedActivityConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MemberStatusChanged> context)
    {
        var statusChange = context.Message;

        _logger.LogInformation(
            "Processing MemberStatusChanged event: {DisplayName} changed from {Previous} to {New}",
            statusChange.DisplayName,
            statusChange.PreviousStatus,
            statusChange.NewStatus);

        var initials = GetInitials(statusChange.DisplayName);
        var avatarColor = GenerateAvatarColor(statusChange.UserId);

        var statusText = FormatStatusText(statusChange.NewStatus);
        var description = string.IsNullOrWhiteSpace(statusChange.StatusMessage)
            ? $"Status changed from {FormatStatusText(statusChange.PreviousStatus)} to {statusText}"
            : statusChange.StatusMessage;

        var activityItem = new ActivityFeedItemEntity
        {
            Id = Guid.NewGuid(),
            ActorUserId = statusChange.UserId,
            ActorDisplayName = statusChange.DisplayName,
            ActorInitials = initials,
            ActorAvatarColor = avatarColor,
            Action = $"is now {statusText}",
            Description = description,
            CreatedAt = statusChange.ChangedAt,
            Category = ActivityCategory.Team
        };

        _db.ActivityFeedItems.Add(activityItem);
        await _db.SaveChangesAsync();

        _logger.LogDebug(
            "Activity feed item created for status change of user {UserId}",
            statusChange.UserId);
    }

    private static string FormatStatusText(PresenceStatus status) => status switch
    {
        PresenceStatus.Online => "online",
        PresenceStatus.Away => "away",
        PresenceStatus.Offline => "offline",
        PresenceStatus.DoNotDisturb => "do not disturb",
        PresenceStatus.InAMeeting => "in a meeting",
        _ => status.ToString().ToLowerInvariant()
    };

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
