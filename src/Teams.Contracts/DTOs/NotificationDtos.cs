using Teams.Contracts.Events;

namespace Teams.Contracts.DTOs;

/// <summary>
/// Represents a user notification (e.g. mention, meeting reminder, system alert).
/// </summary>
public record NotificationDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public NotificationType Type { get; init; }
    public bool IsRead { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
    public Guid? RelatedEntityId { get; init; }
}

/// <summary>
/// Represents a single item in the "Recent Activity" feed shown on the dashboard.
/// Example: "Marco updated the project timeline" with avatar, icon, and timestamp.
/// </summary>
public record ActivityFeedItemDto
{
    public Guid Id { get; init; }
    public Guid ActorUserId { get; init; }
    public string ActorDisplayName { get; init; } = string.Empty;
    public string ActorInitials { get; init; } = string.Empty;
    public string ActorAvatarColor { get; init; } = string.Empty;
    public string Action { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public ActivityCategory Category { get; init; }
}

/// <summary>
/// Paginated result for notification queries.
/// </summary>
public record PaginatedNotificationsDto
{
    public List<NotificationDto> Items { get; init; } = [];
    public int TotalCount { get; init; }
    public int UnreadCount { get; init; }
}

public enum ActivityCategory
{
    Chat,
    Meeting,
    Team,
    File
}
