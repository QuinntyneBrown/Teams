using MediatR;
using TeamSync.Contracts.DTOs;

namespace TeamSync.Services.Team.Handlers;

/// <summary>
/// Updates a team member's presence status (Online, Away, Offline).
/// Publishes a MemberStatusChanged event via MassTransit.
/// </summary>
public record UpdatePresenceCommand(
    Guid UserId,
    string NewStatus,
    string? StatusMessage = null) : IRequest<TeamMemberDto>;

/// <summary>
/// Creates a new team invitation for the specified email address.
/// Publishes a MemberInvited event via MassTransit.
/// </summary>
public record InviteMemberCommand(
    string Email,
    Guid InvitedByUserId,
    string? PersonalMessage = null) : IRequest<TeamInvitationDto>;
