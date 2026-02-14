namespace Teams.Contracts.Events;

/// <summary>
/// Published when a notification should be delivered to one or more users.
/// Consumers can route to push notifications, email, in-app badges, etc.
/// </summary>
public record NotificationCreated
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid NotificationId { get; init; }
    public NotificationType Type { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Body { get; init; } = string.Empty;
    public List<Guid> RecipientIds { get; init; } = [];
    public Guid? TriggeredByUserId { get; init; }
    public string? TriggeredByDisplayName { get; init; }
    public string? ActionUrl { get; init; }
    public Guid? RelatedEntityId { get; init; }
    public string? RelatedEntityType { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}

public enum NotificationType
{
    MessageMention,
    MeetingReminder,
    MeetingInvite,
    MeetingCancellation,
    TeamInvite,
    MemberJoined,
    ChannelActivity,
    DirectMessage,
    SystemAlert
}
