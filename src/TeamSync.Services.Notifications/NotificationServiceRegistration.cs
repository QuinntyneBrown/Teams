using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TeamSync.Services.Notifications.Consumers;
using TeamSync.Services.Notifications.Data;

namespace TeamSync.Services.Notifications;

/// <summary>
/// Provides extension methods to register the Notification microservice
/// dependencies (DbContext, MediatR handlers, MassTransit consumers).
/// </summary>
public static class NotificationServiceRegistration
{
    /// <summary>
    /// Registers the NotificationDbContext with SQLite, MediatR handlers from
    /// this assembly, and ensures the database is created on startup.
    /// </summary>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="sqliteConnectionString">
    /// SQLite connection string. Defaults to "Data Source=notifications.db" if not provided.
    /// </param>
    /// <returns>The service collection for chaining.</returns>
    public static IServiceCollection AddNotificationService(
        this IServiceCollection services,
        string sqliteConnectionString = "Data Source=notifications.db")
    {
        // Register EF Core with SQLite
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseSqlite(sqliteConnectionString));

        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(NotificationServiceRegistration).Assembly));

        // Ensure the SQLite database and tables are created on startup
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
        db.Database.EnsureCreated();

        return services;
    }

    /// <summary>
    /// Returns the MassTransit consumer types from this assembly so they can be
    /// registered with the bus configurator in the host project.
    /// <example>
    /// <code>
    /// services.AddTeamSyncDefaults(configuration, bus =>
    /// {
    ///     bus.AddConsumers(NotificationServiceRegistration.GetConsumerTypes());
    /// });
    /// </code>
    /// </example>
    /// </summary>
    public static Type[] GetConsumerTypes() =>
    [
        typeof(MessageSentActivityConsumer),
        typeof(MeetingCreatedActivityConsumer),
        typeof(MemberStatusChangedActivityConsumer)
    ];
}
