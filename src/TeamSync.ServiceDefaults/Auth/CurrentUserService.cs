using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace TeamSync.ServiceDefaults.Auth;

/// <summary>
/// Defines a service for accessing the current authenticated user's information.
/// </summary>
public interface ICurrentUserService
{
    /// <summary>The authenticated user's unique identifier (subject claim).</summary>
    string? UserId { get; }

    /// <summary>The authenticated user's email address.</summary>
    string? Email { get; }

    /// <summary>The authenticated user's display name.</summary>
    string? DisplayName { get; }

    /// <summary>Whether the current request is from an authenticated user.</summary>
    bool IsAuthenticated { get; }

    /// <summary>The roles assigned to the current user.</summary>
    IEnumerable<string> Roles { get; }
}

/// <summary>
/// Extracts current user information from the HttpContext's ClaimsPrincipal.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public string? UserId =>
        User?.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

    public string? Email =>
        User?.FindFirstValue(ClaimTypes.Email)
        ?? User?.FindFirstValue(JwtRegisteredClaimNames.Email);

    public string? DisplayName =>
        User?.FindFirstValue("display_name")
        ?? User?.FindFirstValue(ClaimTypes.Name);

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated ?? false;

    public IEnumerable<string> Roles =>
        User?.FindAll(ClaimTypes.Role).Select(c => c.Value) ?? Enumerable.Empty<string>();
}
