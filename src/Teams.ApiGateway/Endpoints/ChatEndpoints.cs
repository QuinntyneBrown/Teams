using MediatR;
using Teams.Services.Chat.Handlers;

namespace Teams.ApiGateway.Endpoints;

public static class ChatEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/chat").WithTags("Chat");

        group.MapGet("/channels", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetChannelsQuery())));

        group.MapPost("/channels", async (CreateChannelRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new CreateChannelCommand(
                request.Name, request.Description, request.CreatedByUserId, request.CreatedByDisplayName));
            return Results.Created($"/api/chat/channels/{result.Id}", result);
        });

        group.MapGet("/channels/{channelId:guid}/messages", async (
            Guid channelId, IMediator mediator, int pageSize = 50, Guid? before = null) =>
            Results.Ok(await mediator.Send(new GetChannelMessagesQuery(channelId, pageSize, before))));

        group.MapPost("/channels/{channelId:guid}/messages", async (
            Guid channelId, SendMessageRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new SendMessageCommand(
                channelId, request.SenderId, request.SenderDisplayName, request.Content));
            return Results.Created($"/api/chat/channels/{channelId}/messages/{result.Id}", result);
        });
    }

    public record CreateChannelRequest(string Name, string Description, Guid CreatedByUserId, string CreatedByDisplayName);
    public record SendMessageRequest(Guid SenderId, string SenderDisplayName, string Content);
}
