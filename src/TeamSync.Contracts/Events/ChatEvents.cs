namespace TeamSync.Contracts.Events;

/// <summary>
/// Published when a user sends a new message in a channel.
/// </summary>
public record MessageSent
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MessageId { get; init; }
    public Guid ChannelId { get; init; }
    public string ChannelName { get; init; } = string.Empty;
    public Guid SenderId { get; init; }
    public string SenderDisplayName { get; init; } = string.Empty;
    public string Content { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
    public Guid? ParentMessageId { get; init; }
    public List<string> Mentions { get; init; } = [];
    public List<string> AttachmentUrls { get; init; } = [];
}

/// <summary>
/// Published when a user edits an existing message.
/// </summary>
public record MessageEdited
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MessageId { get; init; }
    public Guid ChannelId { get; init; }
    public Guid EditorId { get; init; }
    public string PreviousContent { get; init; } = string.Empty;
    public string NewContent { get; init; } = string.Empty;
    public DateTimeOffset EditedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a user deletes a message.
/// </summary>
public record MessageDeleted
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid MessageId { get; init; }
    public Guid ChannelId { get; init; }
    public Guid DeletedById { get; init; }
    public DateTimeOffset DeletedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a new channel is created (e.g. #design-team, #engineering).
/// </summary>
public record ChannelCreated
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid ChannelId { get; init; }
    public string ChannelName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public Guid CreatedById { get; init; }
    public string CreatedByDisplayName { get; init; } = string.Empty;
    public bool IsPrivate { get; init; }
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a user joins a channel.
/// </summary>
public record UserJoinedChannel
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid ChannelId { get; init; }
    public string ChannelName { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserDisplayName { get; init; } = string.Empty;
    public DateTimeOffset JoinedAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a user leaves a channel.
/// </summary>
public record UserLeftChannel
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid ChannelId { get; init; }
    public string ChannelName { get; init; } = string.Empty;
    public Guid UserId { get; init; }
    public string UserDisplayName { get; init; } = string.Empty;
    public DateTimeOffset LeftAt { get; init; } = DateTimeOffset.UtcNow;
}

/// <summary>
/// Published when a user begins typing in a channel. Used for real-time typing indicators.
/// </summary>
public record TypingStarted
{
    public Guid CorrelationId { get; init; } = Guid.NewGuid();
    public Guid ChannelId { get; init; }
    public Guid UserId { get; init; }
    public string UserDisplayName { get; init; } = string.Empty;
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}
