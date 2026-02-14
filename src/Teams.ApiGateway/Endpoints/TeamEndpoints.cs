using MediatR;
using Teams.Services.Team.Handlers;

namespace Teams.ApiGateway.Endpoints;

public static class TeamEndpoints
{
    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/team").WithTags("Team");

        group.MapGet("/members", async (IMediator mediator, string? status = null, string? search = null) =>
            Results.Ok(await mediator.Send(new GetTeamMembersQuery(status, search))));

        group.MapGet("/timezones", async (IMediator mediator) =>
            Results.Ok(await mediator.Send(new GetTimeZoneCardsQuery())));

        group.MapPut("/members/{userId:guid}/presence", async (
            Guid userId, UpdatePresenceApiRequest request, IMediator mediator) =>
            Results.Ok(await mediator.Send(new UpdatePresenceCommand(userId, request.Status, request.StatusMessage))));

        group.MapPost("/invite", async (InviteMemberApiRequest request, IMediator mediator) =>
        {
            var result = await mediator.Send(new InviteMemberCommand(request.Email, request.InvitedByUserId, request.PersonalMessage));
            return Results.Created($"/api/team/invitations/{result.Id}", result);
        });
    }

    public record UpdatePresenceApiRequest(string Status, string? StatusMessage = null);
    public record InviteMemberApiRequest(string Email, Guid InvitedByUserId, string? PersonalMessage = null);
}
