using MediatR;

namespace Teams.Services.Notifications.Handlers;

/// <summary>
/// Marks a single notification as read for the given user.
/// </summary>
public record MarkNotificationReadCommand(Guid NotificationId, Guid UserId)
    : IRequest<bool>;
