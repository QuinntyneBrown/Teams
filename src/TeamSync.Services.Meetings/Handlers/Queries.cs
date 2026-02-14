using MediatR;

namespace TeamSync.Services.Meetings.Handlers;

public record GetTodayScheduleQuery(Guid UserId, string TimeZoneId = "UTC") : IRequest<TodayScheduleResult>;

public record TodayScheduleResult(List<MeetingDto> Meetings, string TimeZoneId, DateTimeOffset DayStart, DateTimeOffset DayEnd);

public record GetUpcomingMeetingsQuery(Guid UserId, int Count = 3) : IRequest<UpcomingMeetingsResult>;

public record UpcomingMeetingsResult(List<MeetingDto> Meetings);

public record MeetingDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public DateTimeOffset StartTimeUtc { get; init; }
    public DateTimeOffset EndTimeUtc { get; init; }
    public string AccentColor { get; init; } = string.Empty;
    public bool IsCancelled { get; init; }
    public Guid CreatedByUserId { get; init; }
    public List<AttendeeDto> Attendees { get; init; } = [];
}

public record AttendeeDto
{
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string ResponseStatus { get; init; } = string.Empty;
    public string TimeZoneId { get; init; } = string.Empty;
}
