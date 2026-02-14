using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Services.Team.Data;

namespace TeamSync.Services.Team.Handlers;

/// <summary>
/// Handles <see cref="GetTeamMembersQuery"/> by returning all team members,
/// with optional filtering by status (All/Online/Away/Offline) and search term.
/// </summary>
public class GetTeamMembersHandler : IRequestHandler<GetTeamMembersQuery, List<TeamMemberDto>>
{
    private readonly TeamDbContext _db;

    public GetTeamMembersHandler(TeamDbContext db)
    {
        _db = db;
    }

    public async Task<List<TeamMemberDto>> Handle(
        GetTeamMembersQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.TeamMembers.AsQueryable();

        // Filter by status if a specific status is requested
        if (!string.IsNullOrWhiteSpace(request.StatusFilter)
            && !request.StatusFilter.Equals("All", StringComparison.OrdinalIgnoreCase))
        {
            if (Enum.TryParse<MemberStatus>(request.StatusFilter, ignoreCase: true, out var status))
            {
                query = query.Where(m => m.Status == status);
            }
        }

        // Filter by search term across name and role
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var term = request.SearchTerm.ToLower();
            query = query.Where(m =>
                m.DisplayName.ToLower().Contains(term) ||
                m.Role.ToLower().Contains(term));
        }

        var members = await query
            .OrderBy(m => m.DisplayName)
            .ToListAsync(cancellationToken);

        return members.Select(MapToDto).ToList();
    }

    private static TeamMemberDto MapToDto(TeamMember member) => new()
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
