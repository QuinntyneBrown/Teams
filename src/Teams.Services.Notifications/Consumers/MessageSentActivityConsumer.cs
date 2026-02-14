using MassTransit;
using Microsoft.Extensions.Logging;
using Teams.Contracts.DTOs;
using Teams.Contracts.Events;
using Teams.Services.Notifications.Data;

namespace Teams.Services.Notifications.Consumers;

/// <summary>
/// Consumes <see cref="MessageSent"/> events and creates an activity feed item
/// so the message appears in the dashboard "Recent Activity" section.
/// </summary>
public class MessageSentActivityConsumer : IConsumer<MessageSent>
{
    private readonly NotificationDbContext _db;
    private readonly ILogger<MessageSentActivityConsumer> _logger;

    public MessageSentActivityConsumer(
        NotificationDbContext db,
        ILogger<MessageSentActivityConsumer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<MessageSent> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Processing MessageSent event for channel {ChannelName} from {Sender}",
            message.ChannelName,
            message.SenderDisplayName);

        var initials = GetInitials(message.SenderDisplayName);
        var avatarColor = GenerateAvatarColor(message.SenderId);

        var contentPreview = message.Content.Length > 80
            ? string.Concat(message.Content.AsSpan(0, 80), "...")
            : message.Content;

        var activityItem = new ActivityFeedItemEntity
        {
            Id = Guid.NewGuid(),
            ActorUserId = message.SenderId,
            ActorDisplayName = message.SenderDisplayName,
            ActorInitials = initials,
            ActorAvatarColor = avatarColor,
            Action = $"sent a message in #{message.ChannelName}",
            Description = contentPreview,
            CreatedAt = message.Timestamp,
            Category = ActivityCategory.Chat
        };

        _db.ActivityFeedItems.Add(activityItem);
        await _db.SaveChangesAsync();

        _logger.LogDebug("Activity feed item {ActivityId} created for MessageSent", activityItem.Id);
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
