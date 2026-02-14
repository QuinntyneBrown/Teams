using MassTransit;
using MediatR;
using Teams.Contracts.DTOs;
using Teams.Contracts.Events;
using Teams.Services.Chat.Data;

namespace Teams.Services.Chat.Handlers;

public class SendMessageHandler : IRequestHandler<SendMessageCommand, MessageDto>
{
    private readonly ChatDbContext _db;
    private readonly IPublishEndpoint _publishEndpoint;

    public SendMessageHandler(ChatDbContext db, IPublishEndpoint publishEndpoint)
    {
        _db = db;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<MessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        var message = new Message
        {
            Id = Guid.NewGuid(),
            ChannelId = request.ChannelId,
            SenderId = request.SenderId,
            SenderDisplayName = request.SenderDisplayName,
            Content = request.Content,
            SentAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync(cancellationToken);

        var channel = await _db.Channels.FindAsync(new object[] { request.ChannelId }, cancellationToken);

        await _publishEndpoint.Publish(new MessageSent
        {
            MessageId = message.Id,
            ChannelId = message.ChannelId,
            ChannelName = channel?.Name ?? string.Empty,
            SenderId = message.SenderId,
            SenderDisplayName = message.SenderDisplayName,
            Content = message.Content,
            Timestamp = message.SentAt
        }, cancellationToken);

        return new MessageDto
        {
            Id = message.Id,
            ChannelId = message.ChannelId,
            SenderId = message.SenderId,
            SenderDisplayName = message.SenderDisplayName,
            Content = message.Content,
            SentAt = message.SentAt,
            EditedAt = message.EditedAt,
            IsDeleted = message.IsDeleted
        };
    }
}
