using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Services.Meetings.Data;

namespace TeamSync.Services.Meetings.Handlers;

public class GetUpcomingMeetingsHandler : IRequestHandler<GetUpcomingMeetingsQuery, UpcomingMeetingsResult>
{
    private readonly MeetingDbContext _db;

    public GetUpcomingMeetingsHandler(MeetingDbContext db)
    {
        _db = db;
    }

    public async Task<UpcomingMeetingsResult> Handle(GetUpcomingMeetingsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var meetings = await _db.Meetings
            .Include(m => m.Attendees)
            .Where(m => !m.IsCancelled)
            .Where(m => m.Attendees.Any(a => a.UserId == request.UserId))
            .Where(m => m.EndTimeUtc > now)
            .OrderBy(m => m.StartTimeUtc)
            .Take(request.Count)
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

        return new UpcomingMeetingsResult(dtos);
    }
}
