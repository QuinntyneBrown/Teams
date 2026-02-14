namespace TeamSync.Contracts.DTOs;

/// <summary>
/// Represents a team member with presence and profile information.
/// </summary>
public record TeamMemberDto
{
    public Guid Id { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Role { get; init; } = string.Empty;
    public string Initials { get; init; } = string.Empty;
    public string AvatarColor { get; init; } = string.Empty;
    public string TimeZoneId { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset LastSeenAt { get; init; }
    public DateTimeOffset JoinedAt { get; init; }
}

/// <summary>
/// Represents a pending team invitation.
/// </summary>
public record TeamInvitationDto
{
    public Guid Id { get; init; }
    public string Email { get; init; } = string.Empty;
    public Guid InvitedByUserId { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
}

/// <summary>
/// Represents a time zone group with its members and current local time.
/// </summary>
public record TimeZoneGroupCardDto
{
    public string TimeZoneId { get; init; } = string.Empty;
    public string DisplayLabel { get; init; } = string.Empty;
    public string CurrentLocalTime { get; init; } = string.Empty;
    public string UtcOffset { get; init; } = string.Empty;
    public List<TeamMemberDto> Members { get; init; } = [];
}

/// <summary>
/// Request payload to invite a new member to the team.
/// </summary>
public record InviteMemberRequest
{
    public string Email { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string Role { get; init; } = "Member";
    public string? PersonalMessage { get; init; }
}

/// <summary>
/// Request payload to update a user's presence/status.
/// </summary>
public record UpdatePresenceRequest
{
    public string Status { get; init; } = string.Empty;
    public string? StatusMessage { get; init; }
}
