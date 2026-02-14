namespace TeamSync.Contracts.Events;

/// <summary>
/// Published when a team member sends an invite to a new user.
/// </summary>
public record MemberInvited
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid InvitationId { get; init; }
    public Guid TeamId { get; init; }
    public string InviteeEmail { get; init; } = string.Empty;
    public string? InviteeDisplayName { get; init; }
    public Guid InvitedById { get; init; }
    public string InvitedByDisplayName { get; init; } = string.Empty;
    public string? PersonalMessage { get; init; }
    public DateTimeOffset InvitedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when an invited user accepts and joins the team.
/// </summary>
public record MemberJoined
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string? AvatarUrl { get; init; }
    public string Role { get; init; } = "Member";
    public string TimeZone { get; init; } = "UTC";
    public DateTimeOffset JoinedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a member leaves or is removed from the team.
/// </summary>
public record MemberLeft
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public MemberDepartureReason Reason { get; init; }
    public Guid? RemovedById { get; init; }
    public DateTimeOffset LeftAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a member's presence status changes (online, away, offline).
/// </summary>
public record MemberStatusChanged
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid TeamId { get; init; }
    public Guid UserId { get; init; }
    public string DisplayName { get; init; } = string.Empty;
    public PresenceStatus PreviousStatus { get; init; }
    public PresenceStatus NewStatus { get; init; }
    public string? StatusMessage { get; init; }
    public DateTimeOffset ChangedAt { get; init; } = DateTimeOffset.UtcNow;
}

public enum PresenceStatus
{
    Online,
    Away,
    Offline,
    DoNotDisturb,
    InAMeeting
}

public enum MemberDepartureReason
{
    Voluntary,
    Removed,
    AccountDeactivated
}
