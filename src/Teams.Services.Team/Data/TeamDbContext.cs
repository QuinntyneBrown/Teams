using Microsoft.EntityFrameworkCore;

namespace Teams.Services.Team.Data;

public class TeamDbContext : DbContext
{
    public TeamDbContext(DbContextOptions<TeamDbContext> options) : base(options) { }

    public DbSet<TeamMember> TeamMembers => Set<TeamMember>();
    public DbSet<TeamInvitation> TeamInvitations => Set<TeamInvitation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TeamMember>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.Role).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Initials).IsRequired().HasMaxLength(4);
            entity.Property(e => e.AvatarColor).IsRequired().HasMaxLength(20);
            entity.Property(e => e.TimeZoneId).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            entity.HasData(GetSeedMembers());
        });

        modelBuilder.Entity<TeamInvitation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(320);
            entity.Property(e => e.Status)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);
        });
    }

    private static TeamMember[] GetSeedMembers()
    {
        var now = DateTimeOffset.UtcNow;

        return
        [
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0001-4000-8000-000000000001"),
                DisplayName = "Marco",
                Email = "marco@teamsync.dev",
                Role = "Lead Designer",
                Initials = "MA",
                AvatarColor = "#6366F1",
                TimeZoneId = "America/New_York",
                Status = MemberStatus.Online,
                LastSeenAt = now,
                JoinedAt = now.AddMonths(-6)
            },
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0002-4000-8000-000000000002"),
                DisplayName = "Priya",
                Email = "priya@teamsync.dev",
                Role = "Frontend Dev",
                Initials = "PR",
                AvatarColor = "#EC4899",
                TimeZoneId = "Asia/Kolkata",
                Status = MemberStatus.Online,
                LastSeenAt = now,
                JoinedAt = now.AddMonths(-5)
            },
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0003-4000-8000-000000000003"),
                DisplayName = "Akira",
                Email = "akira@teamsync.dev",
                Role = "Backend Dev",
                Initials = "AK",
                AvatarColor = "#10B981",
                TimeZoneId = "Asia/Tokyo",
                Status = MemberStatus.Away,
                LastSeenAt = now.AddMinutes(-30),
                JoinedAt = now.AddMonths(-8)
            },
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0004-4000-8000-000000000004"),
                DisplayName = "Chen",
                Email = "chen@teamsync.dev",
                Role = "Product Manager",
                Initials = "CH",
                AvatarColor = "#F59E0B",
                TimeZoneId = "Asia/Shanghai",
                Status = MemberStatus.Online,
                LastSeenAt = now,
                JoinedAt = now.AddMonths(-10)
            },
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0005-4000-8000-000000000005"),
                DisplayName = "Lisa",
                Email = "lisa@teamsync.dev",
                Role = "UX Researcher",
                Initials = "LI",
                AvatarColor = "#8B5CF6",
                TimeZoneId = "Europe/London",
                Status = MemberStatus.Offline,
                LastSeenAt = now.AddHours(-3),
                JoinedAt = now.AddMonths(-4)
            },
            new TeamMember
            {
                Id = Guid.Parse("a1b2c3d4-0006-4000-8000-000000000006"),
                DisplayName = "Diego",
                Email = "diego@teamsync.dev",
                Role = "DevOps",
                Initials = "DI",
                AvatarColor = "#EF4444",
                TimeZoneId = "America/Sao_Paulo",
                Status = MemberStatus.Away,
                LastSeenAt = now.AddMinutes(-15),
                JoinedAt = now.AddMonths(-7)
            }
        ];
    }
}

public class TeamMember
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Initials { get; set; } = string.Empty;
    public string AvatarColor { get; set; } = string.Empty;
    public string TimeZoneId { get; set; } = string.Empty;
    public MemberStatus Status { get; set; }
    public DateTimeOffset LastSeenAt { get; set; }
    public DateTimeOffset JoinedAt { get; set; }
}

public class TeamInvitation
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid InvitedByUserId { get; set; }
    public InvitationStatus Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}

public enum MemberStatus
{
    Online,
    Away,
    Offline
}

public enum InvitationStatus
{
    Pending,
    Accepted,
    Declined
}
