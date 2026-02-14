using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Teams.Services.Team.Data;

namespace Teams.Services.Team;

/// <summary>
/// Registers all Team microservice dependencies: EF Core DbContext (SQLite),
/// MediatR handlers, and ensures the database is created on startup.
/// </summary>
public static class TeamServiceRegistration
{
    public static IServiceCollection AddTeamService(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Register EF Core with SQLite
        var connectionString = configuration.GetConnectionString("TeamDb")
            ?? "Data Source=teamsync_team.db";

        services.AddDbContext<TeamDbContext>(options =>
            options.UseSqlite(connectionString));

        // Register MediatR handlers from this assembly
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(TeamServiceRegistration).Assembly));

        // Ensure the database and tables are created at startup
        using var scope = services.BuildServiceProvider().CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<TeamDbContext>();
        db.Database.EnsureCreated();

        return services;
    }
}
