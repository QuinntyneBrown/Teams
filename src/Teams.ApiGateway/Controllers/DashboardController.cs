using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teams.Services.Meetings.Handlers;
using Teams.Services.Notifications.Handlers;
using Teams.Services.Team.Handlers;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/dashboard")]
[Tags("Dashboard")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    // Aggregated dashboard endpoint
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetDashboard(Guid userId, string timeZoneId = "UTC")
    {
        // Fan out queries in parallel
        var meetingsTask = mediator.Send(new GetUpcomingMeetingsQuery(userId, 3));
        var activityTask = mediator.Send(new GetActivityFeedQuery(10));
        var membersTask = mediator.Send(new GetTeamMembersQuery());
        var timeZonesTask = mediator.Send(new GetTimeZoneCardsQuery());

        await Task.WhenAll(meetingsTask, activityTask, membersTask, timeZonesTask);

        var meetings = meetingsTask.Result;
        var activity = activityTask.Result;
        var members = membersTask.Result;
        var timeZones = timeZonesTask.Result;

        var onlineCount = members.Count(m => m.Status == "Online");

        return Ok(new
        {
            UserId = userId,
            UpcomingMeetings = meetings.Meetings,
            RecentActivity = activity,
            TeamTimeZones = timeZones,
            OnlineTeamMemberCount = onlineCount,
            TotalTeamMemberCount = members.Count,
            GeneratedAt = DateTimeOffset.UtcNow
        });
    }
}
