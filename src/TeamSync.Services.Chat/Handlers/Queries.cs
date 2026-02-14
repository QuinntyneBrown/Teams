using MediatR;
using TeamSync.Contracts.DTOs;

namespace TeamSync.Services.Chat.Handlers;

public record GetChannelMessagesQuery(
    Guid ChannelId,
    int PageSize = 50,
    Guid? BeforeCursor = null
) : IRequest<List<MessageDto>>;

public record GetChannelsQuery : IRequest<List<ChannelDto>>;
