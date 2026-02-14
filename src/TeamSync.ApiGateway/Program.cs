using MassTransit;
using Serilog;
using TeamSync.ApiGateway.Hubs;
using TeamSync.ApiGateway.Consumers;
using TeamSync.Services.Chat;
using TeamSync.Services.Meetings;
using TeamSync.Services.Notifications;
using TeamSync.Services.Team;
using TeamSync.ServiceDefaults.Auth;
using TeamSync.ServiceDefaults.Extensions;
using TeamSync.ServiceDefaults.Middleware;

var builder = WebApplication.CreateBuilder(args);

// -- Shared defaults (JWT, Serilog, Swagger, health checks) --
builder.Services.AddTeamSyncDefaults(builder.Configuration, bus =>
{
    // Register MassTransit consumers from all services + gateway
    bus.AddConsumers(ChatServiceRegistration.GetConsumerAssembly());
    bus.AddConsumers(typeof(ChatSignalRConsumer).Assembly);
    bus.AddConsumers(NotificationServiceRegistration.GetConsumerTypes());
});

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
    o.SwaggerEndpoint("/swagger/v1/swagger.json", "TeamSync API v1");
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

// -- Map API Endpoints --
TeamSync.ApiGateway.Endpoints.AuthEndpoints.Map(app);
TeamSync.ApiGateway.Endpoints.ChatEndpoints.Map(app);
TeamSync.ApiGateway.Endpoints.MeetingEndpoints.Map(app);
TeamSync.ApiGateway.Endpoints.TeamEndpoints.Map(app);
TeamSync.ApiGateway.Endpoints.NotificationEndpoints.Map(app);
TeamSync.ApiGateway.Endpoints.DashboardEndpoints.Map(app);

// -- Seed data in development --
if (app.Environment.IsDevelopment())
{
    await TeamSync.ApiGateway.Seed.DataSeeder.SeedAsync(app.Services);
}

Log.Information("TeamSync API Gateway started on {Urls}", string.Join(", ", app.Urls));
app.Run();
