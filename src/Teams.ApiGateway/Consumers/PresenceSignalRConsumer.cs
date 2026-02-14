using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Teams.ApiGateway.Hubs;
using Teams.Contracts.Events;

namespace Teams.ApiGateway.Consumers;

/// <summary>
/// Bridges MassTransit MemberStatusChanged events to SignalR PresenceHub clients.
/// </summary>
public class PresenceSignalRConsumer : IConsumer<MemberStatusChanged>
{
    private readonly IHubContext<PresenceHub> _presenceHub;

    public PresenceSignalRConsumer(IHubContext<PresenceHub> presenceHub)
    {
        _presenceHub = presenceHub;
    }

    public async Task Consume(ConsumeContext<MemberStatusChanged> context)
    {
        var evt = context.Message;
        await _presenceHub.Clients.All.SendAsync("UserStatusChanged", new
        {
            evt.UserId,
            evt.DisplayName,
            Status = evt.NewStatus.ToString(),
            evt.StatusMessage,
            evt.ChangedAt
        });
    }
}
