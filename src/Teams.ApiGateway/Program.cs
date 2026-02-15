using MassTransit;
using Serilog;
using Teams.ApiGateway.Hubs;
using Teams.ApiGateway.Consumers;
using Teams.Services.Chat;
using Teams.Services.Meetings;
using Teams.Services.Notifications;
using Teams.Services.Team;
using Teams.ServiceDefaults.Auth;
using Teams.ServiceDefaults.Extensions;
using Teams.ServiceDefaults.Middleware;

var builder = WebApplication.CreateBuilder(args);

// -- Shared defaults (JWT, Serilog, Swagger, health checks) --
builder.Services.AddTeamsDefaults(builder.Configuration, bus =>
{
    // Register MassTransit consumers from all services + gateway
    bus.AddConsumers(ChatServiceRegistration.GetConsumerAssembly());
    bus.AddConsumers(typeof(ChatSignalRConsumer).Assembly);
    bus.AddConsumers(NotificationServiceRegistration.GetConsumerTypes());
});

// -- API Controllers --
builder.Services.AddControllers();

// -- Microservice registrations (DbContexts, MediatR handlers) --
builder.Services.AddChatService(builder.Configuration);
builder.Services.AddMeetingService(builder.Configuration);
builder.Services.AddTeamService(builder.Configuration);
builder.Services.AddNotificationService();

// -- SignalR --
builder.Services.AddSignalR();

// -- CORS for SignalR (needs AllowCredentials) --
builder.Services.AddCors(options =>
{
    options.AddPolicy("SignalRPolicy", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:Origins").Get<string[]>()
                ?? ["http://localhost:3000", "http://localhost:5173", "http://localhost:4200"])
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

var app = builder.Build();

// -- Ensure databases --
app.Services.EnsureChatDatabaseCreated();

// -- Middleware pipeline --
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();

app.UseSwagger();
app.UseSwaggerUI(o =>
{
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "Teams API v1");
    o.RoutePrefix = "swagger";
});

app.UseCors("SignalRPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapHealthChecks("/health");

// -- Map SignalR Hubs --
app.MapHub<ChatHub>("/hubs/chat");
app.MapHub<PresenceHub>("/hubs/presence");
app.MapHub<MeetingHub>("/hubs/meetings");
app.MapHub<NotificationHub>("/hubs/notifications");

// -- Map API Controllers --
app.MapControllers();

// -- Seed data in development --
if (app.Environment.IsDevelopment())
{
    await Teams.ApiGateway.Seed.DataSeeder.SeedAsync(app.Services);
}

Log.Information("Teams API Gateway started on {Urls}", string.Join(", ", app.Urls));
app.Run();
