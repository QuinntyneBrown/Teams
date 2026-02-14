using MassTransit;
using Microsoft.Extensions.Logging;
using Teams.Contracts.Events;

namespace Teams.Services.Team.Consumers;

/// <summary>
/// Placeholder MassTransit consumer for <see cref="MemberStatusChanged"/> events.
/// Can be extended to trigger notifications, update dashboards, or sync with
/// external presence systems.
/// </summary>
public class MemberStatusChangedConsumer : IConsumer<MemberStatusChanged>
{
    private readonly ILogger<MemberStatusChangedConsumer> _logger;

    public MemberStatusChangedConsumer(ILogger<MemberStatusChangedConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<MemberStatusChanged> context)
    {
        var message = context.Message;

        _logger.LogInformation(
            "Member {DisplayName} ({UserId}) status changed from {PreviousStatus} to {NewStatus}",
            message.DisplayName,
            message.UserId,
            message.PreviousStatus,
            message.NewStatus);

        // TODO: Extend with real-time notification dispatch, analytics tracking,
        //       or external system synchronization as needed.

        return Task.CompletedTask;
    }
}
