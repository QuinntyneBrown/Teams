using MediatR;

namespace Teams.Services.Meetings.Handlers;

public record CreateMeetingCommand : IRequest<CreateMeetingResult>
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset StartTimeUtc { get; init; }
    public DateTimeOffset EndTimeUtc { get; init; }
    public string AccentColor { get; init; } = "#6264A7";
    public Guid CreatedByUserId { get; init; }
    public string CreatedByDisplayName { get; init; } = string.Empty;
    public string OrganizerTimeZone { get; init; } = "UTC";
    public List<AttendeeInfo> Attendees { get; init; } = [];
}

public record AttendeeInfo(Guid UserId, string DisplayName, string TimeZoneId = "UTC");

public record CreateMeetingResult(Guid MeetingId, DateTimeOffset CreatedAt);

public record RespondToMeetingCommand : IRequest<RespondToMeetingResult>
{
    public Guid MeetingId { get; init; }
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public Data.ResponseStatus Response { get; init; }
}

public record RespondToMeetingResult(bool Success, string? Error = null);
