using MediatR;
using Teams.Services.Meetings.Handlers;

namespace Teams.ApiGateway.Endpoints;

public static class MeetingEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/meetings").WithTags("Meetings");

        group.MapGet("/today", async (Guid userId, IMediator mediator, string timeZoneId = "UTC") =>
            Results.Ok(await mediator.Send(new GetTodayScheduleQuery(userId, timeZoneId))));

        group.MapGet("/upcoming", async (Guid userId, IMediator mediator, int count = 3) =>
            Results.Ok(await mediator.Send(new GetUpcomingMeetingsQuery(userId, count))));

        group.MapPost("/", async (CreateMeetingApiRequest request, IMediator mediator) =>
        {
            var command = new CreateMeetingCommand
            {
                Title = request.Title,
                Description = request.Description,
                StartTimeUtc = request.StartTimeUtc,
                EndTimeUtc = request.EndTimeUtc,
                AccentColor = request.AccentColor ?? "#6264A7",
                CreatedByUserId = request.CreatedByUserId,
                CreatedByDisplayName = request.CreatedByDisplayName,
                OrganizerTimeZone = request.TimeZone ?? "UTC",
                Attendees = request.Attendees?.Select(a => new AttendeeInfo(a.UserId, a.DisplayName, a.TimeZoneId ?? "UTC")).ToList() ?? []
            };
            var result = await mediator.Send(command);
            return Results.Created($"/api/meetings/{result.MeetingId}", result);
        });

        group.MapPost("/{meetingId:guid}/respond", async (
            Guid meetingId, RespondRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new RespondToMeetingCommand
            {
                MeetingId = meetingId,
                UserId = request.UserId,
                DisplayName = request.DisplayName,
                Response = Enum.Parse<Teams.Services.Meetings.Data.ResponseStatus>(request.Response, ignoreCase: true)
            });
            return result.Success ? Results.Ok(result) : Results.BadRequest(result);
        });
    }

    public record CreateMeetingApiRequest(
        string Title, string? Description, DateTimeOffset StartTimeUtc, DateTimeOffset EndTimeUtc,
        string? AccentColor, Guid CreatedByUserId, string CreatedByDisplayName,
        string? TimeZone, List<AttendeeApiRequest>? Attendees);
    public record AttendeeApiRequest(Guid UserId, string DisplayName, string? TimeZoneId);
    public record RespondRequest(Guid UserId, string DisplayName, string Response);
}
