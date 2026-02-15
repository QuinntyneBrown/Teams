using MediatR;
using Microsoft.AspNetCore.Mvc;
using Teams.Services.Team.Handlers;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/team")]
[Tags("Team")]
public class TeamController(IMediator mediator) : ControllerBase
{
    public record UpdatePresenceApiRequest(string Status, string? StatusMessage = null);
    public record InviteMemberApiRequest(string Email, Guid InvitedByUserId, string? PersonalMessage = null);

    [HttpGet("members")]
    public async Task<IActionResult> GetMembers(string? status = null, string? search = null)
    {
        return Ok(await mediator.Send(new GetTeamMembersQuery(status, search)));
    }

    [HttpGet("timezones")]
    public async Task<IActionResult> GetTimeZones()
    {
        return Ok(await mediator.Send(new GetTimeZoneCardsQuery()));
    }

    [HttpPut("members/{userId:guid}/presence")]
    public async Task<IActionResult> UpdatePresence(Guid userId, [FromBody] UpdatePresenceApiRequest request)
    {
        return Ok(await mediator.Send(new UpdatePresenceCommand(userId, request.Status, request.StatusMessage)));
    }

    [HttpPost("invite")]
    public async Task<IActionResult> InviteMember([FromBody] InviteMemberApiRequest request)
    {
        var result = await mediator.Send(new InviteMemberCommand(request.Email, request.InvitedByUserId, request.PersonalMessage));
        return Created($"/api/team/invitations/{result.Id}", result);
    }
}
