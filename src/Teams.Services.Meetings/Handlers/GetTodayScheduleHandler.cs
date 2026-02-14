using MediatR;
using Microsoft.EntityFrameworkCore;
using Teams.Services.Meetings.Data;

namespace Teams.Services.Meetings.Handlers;

public class GetTodayScheduleHandler : IRequestHandler<GetTodayScheduleQuery, TodayScheduleResult>
{
    private readonly MeetingDbContext _db;

    public GetTodayScheduleHandler(MeetingDbContext db)
    {
        _db = db;
    }

    public async Task<TodayScheduleResult> Handle(GetTodayScheduleQuery request, CancellationToken cancellationToken)
    {
        // Resolve the user's time zone to determine "today"
        TimeZoneInfo tz;
        try
        {
            tz = TimeZoneInfo.FindSystemTimeZoneById(request.TimeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            tz = TimeZoneInfo.Utc;
        }

        var nowInZone = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, tz);
        var todayStart = new DateTimeOffset(nowInZone.Date, tz.GetUtcOffset(nowInZone.Date));
        var todayEnd = todayStart.AddDays(1);

        // Convert to UTC for the database query
        var utcStart = todayStart.ToUniversalTime();
        var utcEnd = todayEnd.ToUniversalTime();

        // Find meetings the user is attending that overlap with today
        var meetings = await _db.Meetings
            .Include(m => m.Attendees)
            .Where(m => !m.IsCancelled)
            .Where(m => m.Attendees.Any(a => a.UserId == request.UserId))
            .Where(m => m.StartTimeUtc < utcEnd && m.EndTimeUtc > utcStart)
            .OrderBy(m => m.StartTimeUtc)
            .ToListAsync(cancellationToken);

        var dtos = meetings.Select(m => new MeetingDto
        {
            Id = m.Id,
            Title = m.Title,
            Description = m.Description,
            StartTimeUtc = m.StartTimeUtc,
            EndTimeUtc = m.EndTimeUtc,
            AccentColor = m.AccentColor,
            IsCancelled = m.IsCancelled,
            CreatedByUserId = m.CreatedByUserId,
            Attendees = m.Attendees.Select(a => new AttendeeDto
            {
                UserId = a.UserId,
                DisplayName = a.DisplayName,
                ResponseStatus = a.ResponseStatus.ToString(),
                TimeZoneId = a.TimeZoneId
            }).ToList()
        }).ToList();

        return new TodayScheduleResult(dtos, request.TimeZoneId, todayStart, todayEnd);
    }
}
