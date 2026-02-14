using Microsoft.EntityFrameworkCore;
using TeamSync.Contracts.DTOs;
using TeamSync.Contracts.Events;

namespace TeamSync.Services.Notifications.Data;

public class NotificationDbContext : DbContext
{
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options)
        : base(options)
    {
    }

    public DbSet<NotificationEntity> Notifications => Set<NotificationEntity>();
    public DbSet<ActivityFeedItemEntity> ActivityFeedItems => Set<ActivityFeedItemEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<NotificationEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1024);
            entity.Property(e => e.Type).HasConversion<string>().HasMaxLength(32);
            entity.HasIndex(e => new { e.UserId, e.CreatedAt });
            entity.HasIndex(e => new { e.UserId, e.IsRead });
        });

        modelBuilder.Entity<ActivityFeedItemEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ActorDisplayName).IsRequired().HasMaxLength(128);
            entity.Property(e => e.ActorInitials).IsRequired().HasMaxLength(4);
            entity.Property(e => e.ActorAvatarColor).IsRequired().HasMaxLength(16);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1024);
            entity.Property(e => e.Category).HasConversion<string>().HasMaxLength(32);
            entity.HasIndex(e => e.CreatedAt);
        });
    }
}

public class NotificationEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public Guid? RelatedEntityId { get; set; }
}

public class ActivityFeedItemEntity
{
    public Guid Id { get; set; }
    public Guid ActorUserId { get; set; }
    public string ActorDisplayName { get; set; } = string.Empty;
    public string ActorInitials { get; set; } = string.Empty;
    public string ActorAvatarColor { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public ActivityCategory Category { get; set; }
}
