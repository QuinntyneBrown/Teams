using MediatR;
using Teams.Contracts.DTOs;

namespace Teams.Services.Chat.Handlers;

public record SendMessageCommand(
    Guid ChannelId,
    Guid SenderId,
    string SenderDisplayName,
    string Content
) : IRequest<MessageDto>;

public record CreateChannelCommand(
    string Name,
    string Description,
    Guid CreatedByUserId,
    string CreatedByDisplayName
) : IRequest<ChannelDto>;
