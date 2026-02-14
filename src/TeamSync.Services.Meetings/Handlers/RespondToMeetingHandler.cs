using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.Events;
using TeamSync.Services.Meetings.Data;

namespace TeamSync.Services.Meetings.Handlers;

public class RespondToMeetingHandler : IRequestHandler<RespondToMeetingCommand, RespondToMeetingResult>
{
    private readonly MeetingDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public RespondToMeetingHandler(MeetingDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<RespondToMeetingResult> Handle(RespondToMeetingCommand request, CancellationToken cancellationToken)
    {
        var attendee = await _db.MeetingAttendees
            .FirstOrDefaultAsync(a => a.MeetingId == request.MeetingId && a.UserId == request.UserId, cancellationToken);

        if (attendee is null)
        {
            return new RespondToMeetingResult(false, "Attendee not found for this meeting.");
        }

        var meeting = await _db.Meetings
            .FirstOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);

        if (meeting is null || meeting.IsCancelled)
        {
            return new RespondToMeetingResult(false, "Meeting not found or has been cancelled.");
        }

        attendee.ResponseStatus = request.Response;
        attendee.DisplayName = request.DisplayName;
        await _db.SaveChangesAsync(cancellationToken);

        // Map internal ResponseStatus to contract AttendeeResponseStatus
        var contractResponse = request.Response switch
        {
            ResponseStatus.Accepted => AttendeeResponseStatus.Accepted,
            ResponseStatus.Declined => AttendeeResponseStatus.Declined,
            ResponseStatus.Tentative => AttendeeResponseStatus.Tentative,
            _ => AttendeeResponseStatus.Tentative
        };

        await _publishEndpoint.Publish(new AttendeeResponded
        {
            MeetingId = request.MeetingId,
            AttendeeId = request.UserId,
            AttendeeDisplayName = request.DisplayName,
            Response = contractResponse,
            RespondedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        return new RespondToMeetingResult(true);
    }
}
