namespace TeamSync.Contracts.DTOs;

/// <summary>
/// Aggregated data for the home dashboard view, including greeting,
/// upcoming meetings, team time zones, and recent activity.
/// </summary>
public record DashboardDto
{
    public Guid UserId { get; init; }
    public string GreetingName { get; init; } = string.Empty;
    public string GreetingMessage { get; init; } = string.Empty;
    public List<TimeZoneCardDto> TeamTimeZones { get; init; } = [];
    public List<MeetingDto> UpcomingMeetings { get; init; } = [];
    public List<ActivityFeedItemDto> RecentActivity { get; init; } = [];
    public int UnreadNotificationCount { get; init; }
    public int OnlineTeamMemberCount { get; init; }
    public int TotalTeamMemberCount { get; init; }
    public DateTimeOffset GeneratedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Represents a team member's time zone card shown on the dashboard,
/// displaying current local time for distributed team awareness.
/// </summary>
public record TimeZoneCardDto
{
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string TimeZoneId { get; init; } = "UTC";
    public string TimeZoneDisplayName { get; init; } = string.Empty;
    public DateTimeOffset CurrentLocalTime { get; init; }
    public double UtcOffsetHours { get; init; }
    public string? City { get; init; }
}
