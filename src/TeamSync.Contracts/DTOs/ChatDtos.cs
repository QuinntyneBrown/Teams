namespace TeamSync.Contracts.DTOs;

public record MessageDto
{
    public Guid Id { get; init; }
    public Guid ChannelId { get; init; }
    public Guid SenderId { get; init; }
    public string SenderDisplayName { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTimeOffset SentAt { get; init; }
    public DateTimeOffset? EditedAt { get; init; }
    public bool IsDeleted { get; init; }
}

public record ChannelDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset CreatedAt { get; init; }
    public Guid CreatedByUserId { get; init; }
}
