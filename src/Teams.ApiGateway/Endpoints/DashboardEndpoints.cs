using MediatR;
using Teams.Contracts.DTOs;
using Teams.Services.Chat.Handlers;
using Teams.Services.Meetings.Handlers;
using Teams.Services.Notifications.Handlers;
using Teams.Services.Team.Handlers;

namespace Teams.ApiGateway.Endpoints;

public static class DashboardEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/dashboard").WithTags("Dashboard");

        // Aggregated dashboard endpoint
        group.MapGet("/{userId:guid}", async (Guid userId, IMediator mediator, string timeZoneId = "UTC") =>
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

            return Results.Ok(new
            {
                UserId = userId,
                UpcomingMeetings = meetings.Meetings,
                RecentActivity = activity,
                TeamTimeZones = timeZones,
                OnlineTeamMemberCount = onlineCount,
                TotalTeamMemberCount = members.Count,
                GeneratedAt = DateTimeOffset.UtcNow
            });
        });
    }
}
