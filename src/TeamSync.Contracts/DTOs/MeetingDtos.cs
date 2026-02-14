namespace TeamSync.Contracts.DTOs;

using TeamSync.Contracts.Events;

/// <summary>
/// Represents a scheduled meeting on the calendar.
/// </summary>
public record MeetingDto
{
    public Guid MeetingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid OrganizerId { get; init; }
    public string OrganizerDisplayName { get; init; } = string.Empty;
    public DateTimeOffset StartTimeUtc { get; init; }
    public DateTimeOffset EndTimeUtc { get; init; }
    public string OrganizerTimeZone { get; init; } = "UTC";
    public string? MeetingLink { get; init; }
    public bool IsRecurring { get; init; }
    public bool IsCancelled { get; init; }
    public List<AttendeeDto> Attendees { get; init; } = [];
    public DateTimeOffset CreatedAt { get; init; }
}

/// <summary>
/// Request payload to schedule a new meeting.
/// </summary>
public record CreateMeetingRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset StartTimeUtc { get; init; }
    public DateTimeOffset EndTimeUtc { get; init; }
    public string TimeZone { get; init; } = "UTC";
    public List<Guid> AttendeeIds { get; init; } = [];
    public bool IsRecurring { get; init; }
}

/// <summary>
/// Represents a meeting attendee and their RSVP status.
/// </summary>
public record AttendeeDto
{
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string Email { get; init; } = string.Empty;
    public AttendeeResponseStatus ResponseStatus { get; init; }
    public string TimeZone { get; init; } = "UTC";
}

/// <summary>
/// Represents a block of meetings for a given date range, used by the calendar view.
/// </summary>
public record MeetingScheduleDto
{
    public DateTimeOffset RangeStart { get; init; }
    public DateTimeOffset RangeEnd { get; init; }
    public string ViewerTimeZone { get; init; } = "UTC";
    public List<MeetingDto> Meetings { get; init; } = [];
    public int TotalMeetingsInRange { get; init; }
}
