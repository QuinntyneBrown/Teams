using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Contracts.Events;
using TeamSync.Services.Team.Data;

namespace TeamSync.Services.Team.Handlers;

/// <summary>
/// Handles <see cref="UpdatePresenceCommand"/> by updating the member's status
/// in the database and publishing a <see cref="MemberStatusChanged"/> event
/// via MassTransit.
/// </summary>
public class UpdatePresenceHandler : IRequestHandler<UpdatePresenceCommand, TeamMemberDto>
{
    private readonly TeamDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdatePresenceHandler(TeamDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<TeamMemberDto> Handle(
        UpdatePresenceCommand request,
        CancellationToken cancellationToken)
    {
        var member = await _db.TeamMembers
            .FirstOrDefaultAsync(m => m.Id == request.UserId, cancellationToken)
            ?? throw new InvalidOperationException($"Team member {request.UserId} not found.");

        if (!Enum.TryParse<MemberStatus>(request.NewStatus, ignoreCase: true, out var newStatus))
        {
            throw new ArgumentException(
                $"Invalid status '{request.NewStatus}'. Valid values: Online, Away, Offline.");
        }

        var previousStatus = member.Status;
        member.Status = newStatus;
        member.LastSeenAt = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        // Publish presence change event
        await _publishEndpoint.Publish(new MemberStatusChanged
        {
            UserId = member.Id,
            DisplayName = member.DisplayName,
            PreviousStatus = MapToContractStatus(previousStatus),
            NewStatus = MapToContractStatus(newStatus),
            StatusMessage = request.StatusMessage,
            ChangedAt = DateTimeOffset.UtcNow
        }, cancellationToken);

        return new TeamMemberDto
        {
            Id = member.Id,
            DisplayName = member.DisplayName,
            Email = member.Email,
            Role = member.Role,
            Initials = member.Initials,
            AvatarColor = member.AvatarColor,
            TimeZoneId = member.TimeZoneId,
            Status = member.Status.ToString(),
            LastSeenAt = member.LastSeenAt,
            JoinedAt = member.JoinedAt
        };
    }

    private static PresenceStatus MapToContractStatus(MemberStatus status) => status switch
    {
        MemberStatus.Online => PresenceStatus.Online,
        MemberStatus.Away => PresenceStatus.Away,
        MemberStatus.Offline => PresenceStatus.Offline,
        _ => PresenceStatus.Offline
    };
}
