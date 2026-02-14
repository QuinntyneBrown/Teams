using MassTransit;
using MediatR;
using Teams.Contracts.DTOs;
using Teams.Contracts.Events;
using Teams.Services.Chat.Data;

namespace Teams.Services.Chat.Handlers;

public class CreateChannelHandler : IRequestHandler<CreateChannelCommand, ChannelDto>
{
    private readonly ChatDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateChannelHandler(ChatDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<ChannelDto> Handle(CreateChannelCommand request, CancellationToken cancellationToken)
    {
        var channel = new Channel
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByUserId = request.CreatedByUserId
        };

        _db.Channels.Add(channel);
        await _db.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new ChannelCreated
        {
            ChannelId = channel.Id,
            ChannelName = channel.Name,
            Description = channel.Description,
            CreatedById = channel.CreatedByUserId,
            CreatedByDisplayName = request.CreatedByDisplayName,
            CreatedAt = channel.CreatedAt
        }, cancellationToken);

        return new ChannelDto
        {
            Id = channel.Id,
            Name = channel.Name,
            Description = channel.Description,
            CreatedAt = channel.CreatedAt,
            CreatedByUserId = channel.CreatedByUserId
        };
    }
}
