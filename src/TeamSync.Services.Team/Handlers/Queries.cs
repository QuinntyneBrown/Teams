using MediatR;
using TeamSync.Contracts.DTOs;

namespace TeamSync.Services.Team.Handlers;

/// <summary>
/// Retrieves team members with optional status and search filtering.
/// StatusFilter: "All" (default), "Online", "Away", "Offline".
/// SearchTerm: filters by name or role (case-insensitive).
/// </summary>
public record GetTeamMembersQuery(string? StatusFilter = null, string? SearchTerm = null)
    : IRequest<List<TeamMemberDto>>;

/// <summary>
/// Returns team members grouped by time zone with current local time
/// for the "Team Time Zones" dashboard section.
/// </summary>
public record GetTimeZoneCardsQuery : IRequest<List<TimeZoneGroupCardDto>>;
