using MediatR;
using Microsoft.EntityFrameworkCore;
using Teams.Contracts.DTOs;
using Teams.Services.Notifications.Data;

namespace Teams.Services.Notifications.Handlers;

/// <summary>
/// Handles <see cref="GetNotificationsQuery"/> by returning a paginated list
/// of notifications for the specified user, ordered newest-first.
/// </summary>
public class GetNotificationsHandler
    : IRequestHandler<GetNotificationsQuery, PaginatedNotificationsDto>
{
    private readonly NotificationDbContext _db;

    public GetNotificationsHandler(NotificationDbContext db)
    {
        _db = db;
    }

    public async Task<PaginatedNotificationsDto> Handle(
        GetNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _db.Notifications
            .Where(n => n.UserId == request.UserId)
            .OrderByDescending(n => n.CreatedAt);

        var totalCount = await query.CountAsync(cancellationToken);
        var unreadCount = await query.CountAsync(n => !n.IsRead, cancellationToken);

        var items = await query
            .Skip(request.Page * request.PageSize)
            .Take(request.PageSize)
            .Select(n => new NotificationDto
            {
                Id = n.Id,
                UserId = n.UserId,
                Title = n.Title,
                Description = n.Description,
                Type = n.Type,
                IsRead = n.IsRead,
                CreatedAt = n.CreatedAt,
                RelatedEntityId = n.RelatedEntityId
            })
            .ToListAsync(cancellationToken);

        return new PaginatedNotificationsDto
        {
            Items = items,
            TotalCount = totalCount,
            UnreadCount = unreadCount
        };
    }
}
