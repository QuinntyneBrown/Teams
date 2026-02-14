using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Services.Team.Data;

namespace TeamSync.Services.Team.Handlers;

/// <summary>
/// Handles <see cref="GetTimeZoneCardsQuery"/> by returning team members grouped
/// by time zone, each group including the current local time and UTC offset.
/// Powers the "Team Time Zones" dashboard section.
/// </summary>
public class GetTimeZoneCardsHandler : IRequestHandler<GetTimeZoneCardsQuery, List<TimeZoneGroupCardDto>>
{
    private readonly TeamDbContext _db;

    public GetTimeZoneCardsHandler(TeamDbContext db)
    {
        _db = db;
    }

    public async Task<List<TimeZoneGroupCardDto>> Handle(
        GetTimeZoneCardsQuery request,
        CancellationToken cancellationToken)
    {
        var members = await _db.TeamMembers
            .OrderBy(m => m.DisplayName)
            .ToListAsync(cancellationToken);

        var utcNow = DateTimeOffset.UtcNow;

        var grouped = members
            .GroupBy(m => m.TimeZoneId)
            .Select(group =>
            {
                var timeZone = GetTimeZoneInfo(group.Key);
                var localTime = TimeZoneInfo.ConvertTime(utcNow, timeZone);
                var offset = timeZone.GetUtcOffset(utcNow);

                return new TimeZoneGroupCardDto
                {
                    TimeZoneId = group.Key,
                    DisplayLabel = FormatDisplayLabel(group.Key),
                    CurrentLocalTime = localTime.ToString("h:mm tt"),
                    UtcOffset = FormatUtcOffset(offset),
                    Members = group.Select(m => new TeamMemberDto
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
                    }).ToList()
                };
            })
            .OrderBy(tz => tz.UtcOffset)
            .ToList();

        return grouped;
    }

    private static TimeZoneInfo GetTimeZoneInfo(string timeZoneId)
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        }
        catch (TimeZoneNotFoundException)
        {
            // Fallback to UTC if the time zone is not found on this system
            return TimeZoneInfo.Utc;
        }
    }

    private static string FormatDisplayLabel(string timeZoneId)
    {
        // Convert IANA identifiers like "America/New_York" to a friendly label
        var parts = timeZoneId.Split('/');
        var city = parts.Length > 1 ? parts[^1] : parts[0];
        return city.Replace("_", " ");
    }

    private static string FormatUtcOffset(TimeSpan offset)
    {
        var sign = offset >= TimeSpan.Zero ? "+" : "-";
        var abs = offset.Duration();
        return abs.Minutes == 0
            ? $"UTC{sign}{abs.Hours}"
            : $"UTC{sign}{abs.Hours}:{abs.Minutes:D2}";
    }
}
