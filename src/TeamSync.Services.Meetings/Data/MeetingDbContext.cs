using Microsoft.EntityFrameworkCore;

namespace TeamSync.Services.Meetings.Data;

public class MeetingDbContext : DbContext
{
    public MeetingDbContext(DbContextOptions<MeetingDbContext> options) : base(options) { }

    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingAttendee> MeetingAttendees => Set<MeetingAttendee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(m => m.Description)
                .HasMaxLength(2000);

            entity.Property(m => m.AccentColor)
                .HasMaxLength(32);

            entity.HasIndex(m => m.StartTimeUtc);
            entity.HasIndex(m => m.CreatedByUserId);
            entity.HasIndex(m => m.IsCancelled);

            entity.HasMany(m => m.Attendees)
                .WithOne(a => a.Meeting)
                .HasForeignKey(a => a.MeetingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<MeetingAttendee>(entity =>
        {
            entity.HasKey(a => a.Id);

            entity.Property(a => a.DisplayName)
                .IsRequired()
                .HasMaxLength(128);

            entity.Property(a => a.TimeZoneId)
                .HasMaxLength(64);

            entity.Property(a => a.ResponseStatus)
                .HasConversion<string>()
                .HasMaxLength(32);

            entity.HasIndex(a => a.UserId);
            entity.HasIndex(a => new { a.MeetingId, a.UserId }).IsUnique();
        });
    }
}

public class Meeting
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTimeOffset StartTimeUtc { get; set; }
    public DateTimeOffset EndTimeUtc { get; set; }
    public string AccentColor { get; set; } = "#6264A7";
    public Guid CreatedByUserId { get; set; }
    public bool IsCancelled { get; set; }
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public List<MeetingAttendee> Attendees { get; set; } = [];
}

public enum ResponseStatus
{
    Pending,
    Accepted,
    Declined,
    Tentative
}

public class MeetingAttendee
{
    public Guid Id { get; set; }
    public Guid MeetingId { get; set; }
    public Guid UserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public ResponseStatus ResponseStatus { get; set; } = ResponseStatus.Pending;
    public string TimeZoneId { get; set; } = "UTC";

    public Meeting Meeting { get; set; } = null!;
}
