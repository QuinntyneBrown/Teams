using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teams.Services.Chat.Handlers;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/chat")]
[Tags("Chat")]
public class ChatController(IMediator mediator) : ControllerBase
{
    public record CreateChannelRequest(string Name, string Description, Guid CreatedByUserId, string CreatedByDisplayName);
    public record SendMessageRequest(Guid SenderId, string SenderDisplayName, string Content);

    [HttpGet("channels")]
    public async Task<IActionResult> GetChannels()
    {
        return Ok(await mediator.Send(new GetChannelsQuery()));
    }

    [HttpPost("channels")]
    public async Task<IActionResult> CreateChannel([FromBody] CreateChannelRequest request)
    {
        var result = await mediator.Send(new CreateChannelCommand(
            request.Name, request.Description, request.CreatedByUserId, request.CreatedByDisplayName));
        return Created($"/api/chat/channels/{result.Id}", result);
    }

    [HttpGet("channels/{channelId:guid}/messages")]
    public async Task<IActionResult> GetChannelMessages(Guid channelId, int pageSize = 50, Guid? before = null)
    {
        return Ok(await mediator.Send(new GetChannelMessagesQuery(channelId, pageSize, before)));
    }

    [HttpPost("channels/{channelId:guid}/messages")]
    public async Task<IActionResult> SendMessage(Guid channelId, [FromBody] SendMessageRequest request)
    {
        var result = await mediator.Send(new SendMessageCommand(
            channelId, request.SenderId, request.SenderDisplayName, request.Content));
        return Created($"/api/chat/channels/{channelId}/messages/{result.Id}", result);
    }
}
