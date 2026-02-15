using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teams.Services.Notifications.Handlers;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/notifications")]
[Tags("Notifications")]
public class NotificationController(IMediator mediator) : ControllerBase
{
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetNotifications(Guid userId, int pageSize = 20, int page = 0)
    {
        return Ok(await mediator.Send(new GetNotificationsQuery(userId, pageSize, page)));
    }

    [HttpPost("{notificationId:guid}/read")]
    public async Task<IActionResult> MarkAsRead(Guid notificationId, Guid userId)
    {
        return Ok(await mediator.Send(new MarkNotificationReadCommand(notificationId, userId)));
    }

    [HttpGet("activity")]
    public async Task<IActionResult> GetActivityFeed(int count = 10)
    {
        return Ok(await mediator.Send(new GetActivityFeedQuery(count)));
    }
}
