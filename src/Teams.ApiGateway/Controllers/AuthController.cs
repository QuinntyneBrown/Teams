using Microsoft.AspNetCore.Mvc;
using Teams.ServiceDefaults.Auth;

namespace Teams.ApiGateway.Controllers;

[ApiController]
[Route("api/auth")]
[Tags("Auth")]
public class AuthController(IJwtTokenService tokenService) : ControllerBase
{
    public record LoginRequest(string Email, string DisplayName);

    // Dev-only login endpoint that issues JWT tokens without password
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var userId = Guid.NewGuid().ToString();
        var token = tokenService.GenerateToken(userId, request.Email, request.DisplayName);
        return Ok(new { Token = token, UserId = userId, DisplayName = request.DisplayName });
    }

    // Login as a specific seeded user (for dev/testing)
    [HttpPost("login-as/{email}")]
    public IActionResult LoginAs(string email)
    {
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
            return NotFound(new { Error = $"No seed user with email '{email}'" });

        var token = tokenService.GenerateToken(user.Id, email, user.Name);
        return Ok(new { Token = token, UserId = user.Id, DisplayName = user.Name });
    }
}
