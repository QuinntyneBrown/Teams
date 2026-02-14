using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Services.Notifications.Data;

namespace TeamSync.Services.Notifications.Handlers;

/// <summary>
/// Handles <see cref="GetActivityFeedQuery"/> by returning the most recent
/// activity feed items for the dashboard "Recent Activity" section.
/// </summary>
public class GetActivityFeedHandler
    : IRequestHandler<GetActivityFeedQuery, List<ActivityFeedItemDto>>
{
    private readonly NotificationDbContext _db;

    public GetActivityFeedHandler(NotificationDbContext db)
    {
        _db = db;
    }

    public async Task<List<ActivityFeedItemDto>> Handle(
        GetActivityFeedQuery request,
        CancellationToken cancellationToken)
    {
        var items = await _db.ActivityFeedItems
            .OrderByDescending(a => a.CreatedAt)
            .Take(request.Count)
            .Select(a => new ActivityFeedItemDto
            {
                Id = a.Id,
                ActorUserId = a.ActorUserId,
                ActorDisplayName = a.ActorDisplayName,
                ActorInitials = a.ActorInitials,
                ActorAvatarColor = a.ActorAvatarColor,
                Action = a.Action,
                Description = a.Description,
                CreatedAt = a.CreatedAt,
                Category = a.Category
            })
            .ToListAsync(cancellationToken);

        return items;
    }
}
