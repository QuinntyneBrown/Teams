using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Services.Notifications.Data;

namespace TeamSync.Services.Notifications.Handlers;

/// <summary>
/// Handles <see cref="MarkNotificationReadCommand"/> by setting the
/// notification's IsRead flag to true. Returns false if the notification
/// does not exist or does not belong to the specified user.
/// </summary>
public class MarkNotificationReadHandler
    : IRequestHandler<MarkNotificationReadCommand, bool>
{
    private readonly NotificationDbContext _db;

    public MarkNotificationReadHandler(NotificationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> Handle(
        MarkNotificationReadCommand request,
        CancellationToken cancellationToken)
    {
        var notification = await _db.Notifications
            .FirstOrDefaultAsync(
                n => n.Id == request.NotificationId && n.UserId == request.UserId,
                cancellationToken);

        if (notification is null)
            return false;

        if (notification.IsRead)
            return true;

        notification.IsRead = true;
        await _db.SaveChangesAsync(cancellationToken);

        return true;
    }
}
