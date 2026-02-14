using MediatR;
using Teams.Services.Notifications.Handlers;

namespace Teams.ApiGateway.Endpoints;

public static class NotificationEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/notifications").WithTags("Notifications");

        group.MapGet("/{userId:guid}", async (Guid userId, IMediator mediator, int pageSize = 20, int page = 0) =>
            Results.Ok(await mediator.Send(new GetNotificationsQuery(userId, pageSize, page))));

        group.MapPost("/{notificationId:guid}/read", async (
            Guid notificationId, Guid userId, IMediator mediator) =>
            Results.Ok(await mediator.Send(new MarkNotificationReadCommand(notificationId, userId))));

        group.MapGet("/activity", async (IMediator mediator, int count = 10) =>
            Results.Ok(await mediator.Send(new GetActivityFeedQuery(count))));
    }
}
