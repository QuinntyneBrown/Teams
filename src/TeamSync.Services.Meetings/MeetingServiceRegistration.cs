using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamSync.Services.Meetings.Data;

namespace TeamSync.Services.Meetings;

public static class MeetingServiceRegistration
{
    /// <summary>
    /// Registers the Meeting microservice dependencies: EF Core with SQLite,
    /// MediatR handlers, and ensures the database is created on startup.
    /// </summary>
    public static IServiceCollection AddMeetingService(this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core with SQLite
        var connectionString = configuration.GetConnectionString("MeetingsDb")
            ?? "Data Source=meetings.db";

        services.AddDbContext<MeetingDbContext>(options =>
            options.UseSqlite(connectionString));

        // MediatR - scan this assembly for handlers
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssemblyContaining<MeetingDbContext>());

        // Ensure the database and tables are created at startup
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<MeetingDbContext>();
        db.Database.EnsureCreated();

        return services;
    }
}
