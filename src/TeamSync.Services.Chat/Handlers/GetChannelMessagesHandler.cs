using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Services.Chat.Data;

namespace TeamSync.Services.Chat.Handlers;

public class GetChannelMessagesHandler : IRequestHandler<GetChannelMessagesQuery, List<MessageDto>>
{
    private readonly ChatDbContext _db;

    public GetChannelMessagesHandler(ChatDbContext db)
    {
        _db = db;
    }

    public async Task<List<MessageDto>> Handle(GetChannelMessagesQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Messages
            .AsNoTracking()
            .Where(m => m.ChannelId == request.ChannelId && !m.IsDeleted);

        if (request.BeforeCursor.HasValue)
        {
            var cursorMessage = await _db.Messages
                .AsNoTracking()
                .Where(m => m.Id == request.BeforeCursor.Value)
                .Select(m => m.SentAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (cursorMessage != default)
            {
                query = query.Where(m => m.SentAt < cursorMessage);
            }
        }

        var messages = await query
            .OrderByDescending(m => m.SentAt)
            .Take(request.PageSize)
            .Select(m => new MessageDto
            {
                Id = m.Id,
                ChannelId = m.ChannelId,
                SenderId = m.SenderId,
                SenderDisplayName = m.SenderDisplayName,
                Content = m.Content,
                SentAt = m.SentAt,
                EditedAt = m.EditedAt,
                IsDeleted = m.IsDeleted
            })
            .ToListAsync(cancellationToken);

        // Return in chronological order (oldest first)
        messages.Reverse();
        return messages;
    }
}
