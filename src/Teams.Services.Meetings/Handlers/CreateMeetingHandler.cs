using MassTransit;
using MediatR;
using Teams.Contracts.Events;
using Teams.Services.Meetings.Data;

namespace Teams.Services.Meetings.Handlers;

public class CreateMeetingHandler : IRequestHandler<CreateMeetingCommand, CreateMeetingResult>
{
    private readonly MeetingDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateMeetingHandler(MeetingDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateMeetingResult> Handle(CreateMeetingCommand request, CancellationToken cancellationToken)
    {
        var meeting = new Meeting
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            StartTimeUtc = request.StartTimeUtc,
            EndTimeUtc = request.EndTimeUtc,
            AccentColor = request.AccentColor,
            CreatedByUserId = request.CreatedByUserId,
            IsCancelled = false,
            CreatedAt = DateTimeOffset.UtcNow,
            Attendees = request.Attendees.Select(a => new MeetingAttendee
            {
                Id = Guid.NewGuid(),
                UserId = a.UserId,
                DisplayName = a.DisplayName,
                TimeZoneId = a.TimeZoneId,
                ResponseStatus = ResponseStatus.Pending
            }).ToList()
        };

        // Add the organizer as an accepted attendee if not already in the list
        if (meeting.Attendees.All(a => a.UserId != request.CreatedByUserId))
        {
            meeting.Attendees.Add(new MeetingAttendee
            {
                Id = Guid.NewGuid(),
                UserId = request.CreatedByUserId,
                DisplayName = request.CreatedByDisplayName,
                TimeZoneId = request.OrganizerTimeZone,
                ResponseStatus = ResponseStatus.Accepted
            });
        }

        _db.Meetings.Add(meeting);
        await _db.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new MeetingCreated
        {
            MeetingId = meeting.Id,
            Title = meeting.Title,
            Description = meeting.Description,
            OrganizerId = request.CreatedByUserId,
            OrganizerDisplayName = request.CreatedByDisplayName,
            StartTimeUtc = meeting.StartTimeUtc,
            EndTimeUtc = meeting.EndTimeUtc,
            OrganizerTimeZone = request.OrganizerTimeZone,
            InvitedAttendeeIds = request.Attendees.Select(a => a.UserId).ToList(),
            CreatedAt = meeting.CreatedAt
        }, cancellationToken);

        return new CreateMeetingResult(meeting.Id, meeting.CreatedAt);
    }
}
