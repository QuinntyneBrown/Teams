using MediatR;
using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Services.Chat.Data;

namespace TeamSync.Services.Chat.Handlers;

public class GetChannelsHandler : IRequestHandler<GetChannelsQuery, List<ChannelDto>>
{
    private readonly ChatDbContext _db;

    public GetChannelsHandler(ChatDbContext db)
    {
        _db = db;
    }

    public async Task<List<ChannelDto>> Handle(GetChannelsQuery request, CancellationToken cancellationToken)
    {
        return await _db.Channels
            .AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new ChannelDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                CreatedByUserId = c.CreatedByUserId
            })
            .ToListAsync(cancellationToken);
    }
}
