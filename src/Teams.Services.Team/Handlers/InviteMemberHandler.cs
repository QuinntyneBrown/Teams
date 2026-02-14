using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Teams.Contracts.DTOs;
using Teams.Contracts.Events;
using Teams.Services.Team.Data;

namespace Teams.Services.Team.Handlers;

/// <summary>
/// Handles <see cref="InviteMemberCommand"/> by creating a new invitation record
/// and publishing a <see cref="MemberInvited"/> event via MassTransit.
/// </summary>
public class InviteMemberHandler : IRequestHandler<InviteMemberCommand, TeamInvitationDto>
{
    private readonly TeamDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public InviteMemberHandler(TeamDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<TeamInvitationDto> Handle(
        InviteMemberCommand request,
        CancellationToken cancellationToken)
    {
        // Check for existing pending invitation
        var existingInvitation = await _db.TeamInvitations
            .FirstOrDefaultAsync(
                i => i.Email == request.Email && i.Status == InvitationStatus.Pending,
                cancellationToken);

        if (existingInvitation is not null)
        {
            throw new InvalidOperationException(
                $"A pending invitation already exists for {request.Email}.");
        }

        // Resolve the inviter's display name
        var inviter = await _db.TeamMembers
            .FirstOrDefaultAsync(m => m.Id == request.InvitedByUserId, cancellationToken);

        var invitation = new TeamInvitation
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            InvitedByUserId = request.InvitedByUserId,
            Status = InvitationStatus.Pending,
            CreatedAt = DateTimeOffset.UtcNow
        };

        _db.TeamInvitations.Add(invitation);
        await _db.SaveChangesAsync(cancellationToken);

        // Publish invitation event
        await _publishEndpoint.Publish(new MemberInvited
        {
            InvitationId = invitation.Id,
            InviteeEmail = invitation.Email,
            InvitedById = invitation.InvitedByUserId,
            InvitedByDisplayName = inviter?.DisplayName ?? "Unknown",
            PersonalMessage = request.PersonalMessage,
            InvitedAt = invitation.CreatedAt
        }, cancellationToken);

        return new TeamInvitationDto
        {
            Id = invitation.Id,
            Email = invitation.Email,
            InvitedByUserId = invitation.InvitedByUserId,
            Status = invitation.Status.ToString(),
            CreatedAt = invitation.CreatedAt
        };
    }
}
