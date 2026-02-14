namespace TeamSync.Contracts.Events;

/// <summary>
/// Published when a new meeting is scheduled on the calendar.
/// </summary>
public record MeetingCreated
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MeetingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid OrganizerId { get; init; }
    public string OrganizerDisplayName { get; init; } = string.Empty;
    public DateTimeOffset StartTimeUtc { get; init; }
    public DateTimeOffset EndTimeUtc { get; init; }
    public string OrganizerTimeZone { get; init; } = "UTC";
    public List<Guid> InvitedAttendeeIds { get; init; } = [];
    public string? MeetingLink { get; init; }
    public bool IsRecurring { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when meeting details (time, title, attendees) are modified.
/// </summary>
public record MeetingUpdated
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MeetingId { get; init; }
    public Guid UpdatedById { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTimeOffset? NewStartTimeUtc { get; init; }
    public DateTimeOffset? NewEndTimeUtc { get; init; }
    public List<Guid>? AddedAttendeeIds { get; init; }
    public List<Guid>? RemovedAttendeeIds { get; init; }
    public DateTimeOffset UpdatedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a meeting is cancelled by the organizer.
/// </summary>
public record MeetingCancelled
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MeetingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public Guid CancelledById { get; init; }
    public string? CancellationReason { get; init; }
    public List<Guid> AffectedAttendeeIds { get; init; } = [];
    public DateTimeOffset CancelledAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published a few minutes before a meeting starts, used to trigger reminders.
/// </summary>
public record MeetingStarting
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MeetingId { get; init; }
    public string Title { get; init; } = string.Empty;
    public DateTimeOffset StartTimeUtc { get; init; }
    public int MinutesUntilStart { get; init; }
    public string? MeetingLink { get; init; }
    public List<Guid> AttendeeIds { get; init; } = [];
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when an attendee accepts, declines, or tentatively accepts a meeting invite.
/// </summary>
public record AttendeeResponded
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MeetingId { get; init; }
    public Guid AttendeeId { get; init; }
    public string AttendeeDisplayName { get; init; } = string.Empty;
    public AttendeeResponseStatus Response { get; init; }
    public DateTimeOffset RespondedAt { get; init; } = DateTimeOffset.UtcNow;
}

public enum AttendeeResponseStatus
{
    Accepted,
    Declined,
    Tentative
}
