using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TeamSync.Services.Chat.Data;

namespace TeamSync.Services.Chat;

public static class ChatServiceRegistration
{
    public static IServiceCollection AddChatService(this IServiceCollection services, IConfiguration configuration)
    {
        // Register ChatDbContext with SQLite
        var connectionString = configuration.GetConnectionString("ChatDb") ?? "Data Source=chat.db";
        services.AddDbContext<ChatDbContext>(options =>
            options.UseSqlite(connectionString));

        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }

    /// <summary>
    /// Returns the assembly containing MassTransit consumers for this service.
    /// Use this when configuring MassTransit to register consumers:
    /// cfg.AddConsumers(ChatServiceRegistration.GetConsumerAssembly());
    /// </summary>
    public static Assembly GetConsumerAssembly() => Assembly.GetExecutingAssembly();

    /// <summary>
    /// Ensures the Chat database is created. Call during application startup.
    /// </summary>
    public static void EnsureChatDatabaseCreated(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ChatDbContext>();
        db.Database.EnsureCreated();
    }
}
