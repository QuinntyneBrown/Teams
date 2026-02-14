using Teams.ServiceDefaults.Auth;

namespace Teams.ApiGateway.Endpoints;

public static class AuthEndpoints
{
    public record LoginRequest(string Email, string DisplayName);

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        // Dev-only login endpoint that issues JWT tokens without password
        group.MapPost("/login", (LoginRequest request, IJwtTokenService tokenService) =>
        {
            var userId = Guid.NewGuid().ToString();
            var token = tokenService.GenerateToken(userId, request.Email, request.DisplayName);
            return Results.Ok(new { Token = token, UserId = userId, DisplayName = request.DisplayName });
        });

        // Login as a specific seeded user (for dev/testing)
        group.MapPost("/login-as/{email}", (string email, IJwtTokenService tokenService) =>
        {
            // Map known seed users
            var users = new Dictionary<string, (string Id, string Name)>
            {
                ["sarah@teamsync.io"] = ("00000000-0000-0000-0000-000000000001", "Sarah"),
                ["marco@teamsync.io"] = ("00000000-0000-0000-0000-000000000002", "Marco"),
                ["priya@teamsync.io"] = ("00000000-0000-0000-0000-000000000003", "Priya"),
                ["akira@teamsync.io"] = ("00000000-0000-0000-0000-000000000004", "Akira"),
                ["chen@teamsync.io"] = ("00000000-0000-0000-0000-000000000005", "Chen"),
                ["lisa@teamsync.io"] = ("00000000-0000-0000-0000-000000000006", "Lisa"),
            };

            if (!users.TryGetValue(email.ToLowerInvariant(), out var user))
                return Results.NotFound(new { Error = $"No seed user with email '{email}'" });

            var token = tokenService.GenerateToken(user.Id, email, user.Name);
            return Results.Ok(new { Token = token, UserId = user.Id, DisplayName = user.Name });
        });
    }
}
