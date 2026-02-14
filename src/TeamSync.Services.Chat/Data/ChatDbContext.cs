using Microsoft.EntityFrameworkCore;

namespace TeamSync.Services.Chat.Data;

public class ChatDbContext : DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<ChannelMember> ChannelMembers => Set<ChannelMember>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Channel>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name).IsRequired().HasMaxLength(200);
            entity.Property(c => c.Description).HasMaxLength(1000);
            entity.Property(c => c.CreatedAt).IsRequired();
            entity.Property(c => c.CreatedByUserId).IsRequired();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.ChannelId).IsRequired();
            entity.Property(m => m.SenderId).IsRequired();
            entity.Property(m => m.SenderDisplayName).IsRequired().HasMaxLength(200);
            entity.Property(m => m.Content).IsRequired();
            entity.Property(m => m.SentAt).IsRequired();
            entity.Property(m => m.IsDeleted).HasDefaultValue(false);

            entity.HasIndex(m => new { m.ChannelId, m.SentAt });
        });

        modelBuilder.Entity<ChannelMember>(entity =>
        {
            entity.HasKey(cm => new { cm.ChannelId, cm.UserId });
            entity.Property(cm => cm.JoinedAt).IsRequired();
        });
    }
}

public class Channel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public Guid CreatedByUserId { get; set; }
}

public class Message
{
    public Guid Id { get; set; }
    public Guid ChannelId { get; set; }
    public Guid SenderId { get; set; }
    public string SenderDisplayName { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset SentAt { get; set; }
    public DateTimeOffset? EditedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class ChannelMember
{
    public Guid ChannelId { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
}
