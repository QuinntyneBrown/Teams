using MediatR;
using TeamSync.Contracts.DTOs;

namespace TeamSync.Services.Notifications.Handlers;

/// <summary>
/// Retrieves paginated notifications for a specific user.
/// </summary>
public record GetNotificationsQuery(Guid UserId, int PageSize = 20, int Page = 0)
    : IRequest<PaginatedNotificationsDto>;

/// <summary>
/// Retrieves the most recent activity feed items for the dashboard.
/// </summary>
public record GetActivityFeedQuery(int Count = 10)
    : IRequest<List<ActivityFeedItemDto>>;
