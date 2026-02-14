using MediatR;
using Microsoft.EntityFrameworkCore;
using Teams.Contracts.DTOs;
using Teams.Services.Team.Data;

namespace Teams.Services.Team.Handlers;

/// <summary>
/// Dedicated search handler that finds team members by name or role.
/// Uses a standalone query type for cases where only search is needed
/// without status filtering.
/// </summary>
public record SearchTeamMembersQuery(string SearchTerm) : IRequest<List<TeamMemberDto>>;

public class SearchTeamMembersHandler : IRequestHandler<SearchTeamMembersQuery, List<TeamMemberDto>>
{
    private readonly TeamDbContext _db;

    public SearchTeamMembersHandler(TeamDbContext db)
    {
        _db = db;
    }

    public async Task<List<TeamMemberDto>> Handle(
        SearchTeamMembersQuery request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            return [];
        }

        var term = request.SearchTerm.ToLower();

        var members = await _db.TeamMembers
            .Where(m =>
                m.DisplayName.ToLower().Contains(term) ||
                m.Role.ToLower().Contains(term))
            .OrderBy(m => m.DisplayName)
            .ToListAsync(cancellationToken);

        return members.Select(m => new TeamMemberDto
        {
            Id = m.Id,
            DisplayName = m.DisplayName,
            Email = m.Email,
            Role = m.Role,
            Initials = m.Initials,
            AvatarColor = m.AvatarColor,
            TimeZoneId = m.TimeZoneId,
            Status = m.Status.ToString(),
            LastSeenAt = m.LastSeenAt,
            JoinedAt = m.JoinedAt
        }).ToList();
    }
}
