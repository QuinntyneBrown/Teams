using System.Text;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Teams.ServiceDefaults.Auth;
using Teams.ServiceDefaults.Middleware;

namespace Teams.ServiceDefaults.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all Teams shared defaults: JWT auth, MassTransit (in-memory),
    /// Serilog, Swagger, CORS, health checks, and common services.
    /// </summary>
    public static IServiceCollection AddTeamsDefaults(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddTeamsDefaults(configuration, _ => { });
    }

    /// <summary>
    /// Registers all Teams shared defaults with an opportunity to configure
    /// MassTransit consumers via <paramref name="configureBus"/>.
    /// </summary>
    public static IServiceCollection AddTeamsDefaults(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> configureBus)
    {
        services.AddTeamsAuthentication(configuration);
        services.AddTeamsMassTransit(configureBus);
        services.AddTeamsSerilog(configuration);
        services.AddTeamsSwagger();
        services.AddTeamsCors();
        services.AddTeamsHealthChecks();
        services.AddTeamsServices();

        return services;
    }

    /// <summary>
    /// Configures JWT Bearer authentication using symmetric key from configuration.
    /// Expects "Jwt:Key", "Jwt:Issuer", and "Jwt:Audience" in configuration.
    /// </summary>
    public static IServiceCollection AddTeamsAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtKey = configuration["Jwt:Key"]
            ?? throw new InvalidOperationException("Jwt:Key is not configured.");
        var jwtIssuer = configuration["Jwt:Issuer"]
            ?? throw new InvalidOperationException("Jwt:Issuer is not configured.");
        var jwtAudience = configuration["Jwt:Audience"]
            ?? throw new InvalidOperationException("Jwt:Audience is not configured.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtIssuer,
                ValidAudience = jwtAudience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                ClockSkew = TimeSpan.FromMinutes(1)
            };
        });

        services.AddAuthorization();

        return services;
    }

    /// <summary>
    /// Configures MassTransit with the InMemory transport.
    /// Use <paramref name="configureBus"/> to register consumers.
    /// </summary>
    public static IServiceCollection AddTeamsMassTransit(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator> configureBus)
    {
        services.AddMassTransit(bus =>
        {
            configureBus(bus);

            bus.UsingInMemory((context, cfg) =>
            {
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Configures Serilog structured logging from configuration.
    /// </summary>
    public static IServiceCollection AddTeamsSerilog(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Teams")
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
            .CreateLogger();

        services.AddSerilog();

        return services;
    }

    /// <summary>
    /// Configures Swagger/OpenAPI with JWT bearer token support.
    /// </summary>
    public static IServiceCollection AddTeamsSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Teams API",
                Version = "v1",
                Description = "Teams Collaboration Platform API"
            });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    /// <summary>
    /// Configures a permissive CORS policy named "AllowAll".
    /// </summary>
    public static IServiceCollection AddTeamsCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        return services;
    }

    /// <summary>
    /// Registers basic health checks.
    /// </summary>
    public static IServiceCollection AddTeamsHealthChecks(this IServiceCollection services)
    {
        services.AddHealthChecks();
        return services;
    }

    /// <summary>
    /// Registers shared application services (JWT token service, current user service).
    /// </summary>
    private static IServiceCollection AddTeamsServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtTokenService, JwtTokenService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddHttpContextAccessor();
        services.AddTransient<ExceptionHandlingMiddleware>();
        services.AddTransient<RequestLoggingMiddleware>();

        return services;
    }
}
