using Microsoft.EntityFrameworkCore;
using Teams.Contracts.DTOs;
using Teams.Services.Chat.Data;
using Teams.Services.Meetings.Data;
using Teams.Services.Notifications.Data;
using Teams.Services.Team.Data;

namespace Teams.ApiGateway.Seed;

public static class DataSeeder
{
    // Fixed GUIDs for seed users (match AuthEndpoints)
    private static readonly Guid SarahId = Guid.Parse("00000000-0000-0000-0000-000000000001");
    private static readonly Guid MarcoId = Guid.Parse("00000000-0000-0000-0000-000000000002");
    private static readonly Guid PriyaId = Guid.Parse("00000000-0000-0000-0000-000000000003");
    private static readonly Guid AkiraId = Guid.Parse("00000000-0000-0000-0000-000000000004");
    private static readonly Guid ChenId = Guid.Parse("00000000-0000-0000-0000-000000000005");
    private static readonly Guid LisaId = Guid.Parse("00000000-0000-0000-0000-000000000006");
    private static readonly Guid DiegoId = Guid.Parse("00000000-0000-0000-0000-000000000007");

    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var sp = scope.ServiceProvider;

        await SeedTeamMembers(sp);
        await SeedChannelsAndMessages(sp);
        await SeedMeetings(sp);
        await SeedActivity(sp);
    }

    private static async Task SeedTeamMembers(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<TeamDbContext>();
        if (await db.TeamMembers.AnyAsync()) return;

        db.TeamMembers.AddRange(
            new TeamMember
            {
                Id = SarahId, DisplayName = "Sarah", Email = "sarah@teamsync.io",
                Role = "Project Manager", Initials = "S", AvatarColor = "#6366F1",
                TimeZoneId = "America/New_York", Status = MemberStatus.Online,
                LastSeenAt = DateTimeOffset.UtcNow
            },
            new TeamMember
            {
                Id = MarcoId, DisplayName = "Marco", Email = "marco@teamsync.io",
                Role = "Lead Designer", Initials = "M", AvatarColor = "#6366F1",
                TimeZoneId = "America/New_York", Status = MemberStatus.Online,
                LastSeenAt = DateTimeOffset.UtcNow
            },
            new TeamMember
            {
                Id = PriyaId, DisplayName = "Priya", Email = "priya@teamsync.io",
                Role = "Frontend Dev", Initials = "P", AvatarColor = "#F43F5E",
                TimeZoneId = "Asia/Kolkata", Status = MemberStatus.Online,
                LastSeenAt = DateTimeOffset.UtcNow
            },
            new TeamMember
            {
                Id = AkiraId, DisplayName = "Akira", Email = "akira@teamsync.io",
                Role = "Backend Dev", Initials = "A", AvatarColor = "#22C55E",
                TimeZoneId = "Asia/Tokyo", Status = MemberStatus.Away,
                LastSeenAt = DateTimeOffset.UtcNow.AddMinutes(-30)
            },
            new TeamMember
            {
                Id = ChenId, DisplayName = "Chen", Email = "chen@teamsync.io",
                Role = "Product Manager", Initials = "C", AvatarColor = "#F59E0B",
                TimeZoneId = "Asia/Shanghai", Status = MemberStatus.Away,
                LastSeenAt = DateTimeOffset.UtcNow.AddHours(-1)
            },
            new TeamMember
            {
                Id = LisaId, DisplayName = "Lisa", Email = "lisa@teamsync.io",
                Role = "UX Researcher", Initials = "L", AvatarColor = "#3B82F6",
                TimeZoneId = "Europe/London", Status = MemberStatus.Online,
                LastSeenAt = DateTimeOffset.UtcNow
            },
            new TeamMember
            {
                Id = DiegoId, DisplayName = "Diego", Email = "diego@teamsync.io",
                Role = "DevOps Engineer", Initials = "D", AvatarColor = "#8B5CF6",
                TimeZoneId = "America/Sao_Paulo", Status = MemberStatus.Offline,
                LastSeenAt = DateTimeOffset.UtcNow.AddHours(-3)
            }
        );
        await db.SaveChangesAsync();
    }

    private static async Task SeedChannelsAndMessages(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<ChatDbContext>();
        if (await db.Channels.AnyAsync()) return;

        var designTeam = new Channel { Id = Guid.NewGuid(), Name = "design-team", Description = "Design team discussions", CreatedByUserId = MarcoId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-30) };
        var general = new Channel { Id = Guid.NewGuid(), Name = "general", Description = "General discussion", CreatedByUserId = SarahId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-60) };
        var engineering = new Channel { Id = Guid.NewGuid(), Name = "engineering", Description = "Engineering topics", CreatedByUserId = AkiraId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-45) };
        var random = new Channel { Id = Guid.NewGuid(), Name = "random", Description = "Off-topic fun", CreatedByUserId = LisaId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-55) };
        var product = new Channel { Id = Guid.NewGuid(), Name = "product", Description = "Product discussions", CreatedByUserId = ChenId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-40) };
        var marketing = new Channel { Id = Guid.NewGuid(), Name = "marketing", Description = "Marketing updates", CreatedByUserId = SarahId, CreatedAt = DateTimeOffset.UtcNow.AddDays(-35) };

        db.Channels.AddRange(designTeam, general, engineering, random, product, marketing);

        // Messages in #design-team matching the UI
        var today = DateTimeOffset.UtcNow.Date;
        db.Messages.AddRange(
            new Message
            {
                Id = Guid.NewGuid(), ChannelId = designTeam.Id, SenderId = MarcoId,
                SenderDisplayName = "Marco", Content = "Hey team! I've updated the design system with the new color palette. Check it out when you get a chance \U0001f3a8",
                SentAt = new DateTimeOffset(today.Year, today.Month, today.Day, 13, 0, 0, TimeSpan.Zero)
            },
            new Message
            {
                Id = Guid.NewGuid(), ChannelId = designTeam.Id, SenderId = PriyaId,
                SenderDisplayName = "Priya", Content = "Looks amazing! The new gradient combinations are really eye-catching. Should we discuss the responsive breakpoints in today's standup?",
                SentAt = new DateTimeOffset(today.Year, today.Month, today.Day, 13, 10, 0, TimeSpan.Zero)
            },
            new Message
            {
                Id = Guid.NewGuid(), ChannelId = designTeam.Id, SenderId = AkiraId,
                SenderDisplayName = "Akira", Content = "Great work on the palette! I've started implementing the theme provider. Quick Q: should we support dynamic theme switching?",
                SentAt = new DateTimeOffset(today.Year, today.Month, today.Day, 13, 15, 0, TimeSpan.Zero)
            },
            new Message
            {
                Id = Guid.NewGuid(), ChannelId = designTeam.Id, SenderId = SarahId,
                SenderDisplayName = "Sarah", Content = "Yes! Let's add it to the agenda. I'll send a calendar invite for all timezones \U0001f30d",
                SentAt = new DateTimeOffset(today.Year, today.Month, today.Day, 13, 20, 0, TimeSpan.Zero)
            }
        );

        // Channel members
        var allUsers = new[] { SarahId, MarcoId, PriyaId, AkiraId, ChenId, LisaId };
        foreach (var channel in new[] { designTeam, general, engineering, random, product, marketing })
        {
            foreach (var userId in allUsers)
            {
                db.ChannelMembers.Add(new ChannelMember
                {
                    ChannelId = channel.Id, UserId = userId, JoinedAt = channel.CreatedAt
                });
            }
        }

        await db.SaveChangesAsync();
    }

    private static async Task SeedMeetings(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<MeetingDbContext>();
        if (await db.Meetings.AnyAsync()) return;

        var today = DateTimeOffset.UtcNow.Date;

        var sprint = new Meeting
        {
            Id = Guid.NewGuid(), Title = "Sprint Planning", Description = "Weekly sprint planning",
            StartTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 14, 0, 0, TimeSpan.Zero),
            EndTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 14, 30, 0, TimeSpan.Zero),
            AccentColor = "#6366F1", CreatedByUserId = SarahId
        };
        var standup = new Meeting
        {
            Id = Guid.NewGuid(), Title = "Cross-Team Standup", Description = "Daily cross-team sync",
            StartTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 18, 0, 0, TimeSpan.Zero),
            EndTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 19, 0, 0, TimeSpan.Zero),
            AccentColor = "#F43F5E", CreatedByUserId = ChenId
        };
        var oneOnOne = new Meeting
        {
            Id = Guid.NewGuid(), Title = "1:1 with Akira", Description = "Weekly 1:1",
            StartTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 20, 30, 0, TimeSpan.Zero),
            EndTimeUtc = new DateTimeOffset(today.Year, today.Month, today.Day, 21, 0, 0, TimeSpan.Zero),
            AccentColor = "#22C55E", CreatedByUserId = SarahId
        };

        db.Meetings.AddRange(sprint, standup, oneOnOne);

        // Attendees
        db.MeetingAttendees.AddRange(
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = sprint.Id, UserId = SarahId, DisplayName = "Sarah", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "America/New_York" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = sprint.Id, UserId = MarcoId, DisplayName = "Marco", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "America/New_York" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = sprint.Id, UserId = PriyaId, DisplayName = "Priya", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "Asia/Kolkata" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = sprint.Id, UserId = AkiraId, DisplayName = "Akira", ResponseStatus = ResponseStatus.Tentative, TimeZoneId = "Asia/Tokyo" },

            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = standup.Id, UserId = SarahId, DisplayName = "Sarah", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "America/New_York" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = standup.Id, UserId = ChenId, DisplayName = "Chen", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "Asia/Shanghai" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = standup.Id, UserId = LisaId, DisplayName = "Lisa", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "Europe/London" },

            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = oneOnOne.Id, UserId = SarahId, DisplayName = "Sarah", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "America/New_York" },
            new MeetingAttendee { Id = Guid.NewGuid(), MeetingId = oneOnOne.Id, UserId = AkiraId, DisplayName = "Akira", ResponseStatus = ResponseStatus.Accepted, TimeZoneId = "Asia/Tokyo" }
        );

        await db.SaveChangesAsync();
    }

    private static async Task SeedActivity(IServiceProvider sp)
    {
        var db = sp.GetRequiredService<NotificationDbContext>();
        if (await db.ActivityFeedItems.AnyAsync()) return;

        db.ActivityFeedItems.AddRange(
            new ActivityFeedItemEntity
            {
                Id = Guid.NewGuid(), ActorUserId = MarcoId, ActorDisplayName = "Marco",
                ActorInitials = "M", ActorAvatarColor = "#6366F1",
                Action = "updated the project timeline", Description = "Sprint 24 milestones adjusted",
                Category = ActivityCategory.Team, CreatedAt = DateTimeOffset.UtcNow.AddMinutes(-45)
            },
            new ActivityFeedItemEntity
            {
                Id = Guid.NewGuid(), ActorUserId = PriyaId, ActorDisplayName = "Priya",
                ActorInitials = "P", ActorAvatarColor = "#F43F5E",
                Action = "completed design review", Description = "Dashboard redesign approved",
                Category = ActivityCategory.Meeting, CreatedAt = DateTimeOffset.UtcNow.AddHours(-2)
            },
            new ActivityFeedItemEntity
            {
                Id = Guid.NewGuid(), ActorUserId = AkiraId, ActorDisplayName = "Akira",
                ActorInitials = "A", ActorAvatarColor = "#22C55E",
                Action = "uploaded new files to shared drive", Description = "API documentation v2.1",
                Category = ActivityCategory.File, CreatedAt = DateTimeOffset.UtcNow.AddHours(-4)
            }
        );

        await db.SaveChangesAsync();
    }
}
