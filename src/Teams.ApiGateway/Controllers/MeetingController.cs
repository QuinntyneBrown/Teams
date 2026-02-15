using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teams.Services.Meetings.Handlers;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/meetings")]
[Tags("Meetings")]
public class MeetingController(IMediator mediator) : ControllerBase
{
    public record CreateMeetingApiRequest(
        string Title, string? Description, DateTimeOffset StartTimeUtc, DateTimeOffset EndTimeUtc,
        string? AccentColor, Guid CreatedByUserId, string CreatedByDisplayName,
        string? TimeZone, List<AttendeeApiRequest>? Attendees);
    public record AttendeeApiRequest(Guid UserId, string DisplayName, string? TimeZoneId);
    public record RespondRequest(Guid UserId, string DisplayName, string Response);

    [HttpGet("today")]
    public async Task<IActionResult> GetTodaySchedule(Guid userId, string timeZoneId = "UTC")
    {
        return Ok(await mediator.Send(new GetTodayScheduleQuery(userId, timeZoneId)));
    }

    [HttpGet("upcoming")]
    public async Task<IActionResult> GetUpcoming(Guid userId, int count = 3)
    {
        return Ok(await mediator.Send(new GetUpcomingMeetingsQuery(userId, count)));
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingApiRequest request)
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
        return Created($"/api/meetings/{result.MeetingId}", result);
    }

    [HttpPost("{meetingId:guid}/respond")]
    public async Task<IActionResult> Respond(Guid meetingId, [FromBody] RespondRequest request)
    {
        var result = await mediator.Send(new RespondToMeetingCommand
        {
            MeetingId = meetingId,
            UserId = request.UserId,
            DisplayName = request.DisplayName,
            Response = Enum.Parse<Teams.Services.Meetings.Data.ResponseStatus>(request.Response, ignoreCase: true)
        });
        return result.Success ? Ok(result) : BadRequest(result);
    }
}
