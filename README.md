# TeamSync

A real-time collaboration platform for distributed teams. TeamSync provides chat, meeting scheduling, team management, and timezone-aware coordination across global workforces.

## Architecture

TeamSync is built as a .NET 8 microservices backend with an Angular frontend.

```
Teams.sln
├── src/
│   ├── Teams.Web/                          # Angular 21 SPA frontend
│   ├── Teams.ApiGateway/                   # API gateway (routes to services)
│   ├── Teams.Services.Chat/               # Chat & channels
│   ├── Teams.Services.Meetings/           # Meeting scheduling
│   ├── Teams.Services.Team/               # Team & member management
│   ├── Teams.Services.Notifications/      # Push/email notifications
│   ├── Teams.Contracts/                   # Shared DTOs & events
│   └── Teams.ServiceDefaults/             # Cross-cutting concerns
├── tests/                                  # Test projects
└── designs/
    └── teams.pen                           # UI designs (Pencil format)
```

### Backend

| Component | Purpose |
|---|---|
| **API Gateway** | Single entry point, routes requests to services |
| **Chat Service** | Channels, direct messages, message history |
| **Meetings Service** | Scheduling, calendar, timezone conversion |
| **Team Service** | Members, roles, presence/status |
| **Notifications Service** | Real-time alerts, email digests |

Key libraries: MassTransit (async messaging), MediatR (CQRS), Entity Framework Core + SQLite, JWT authentication, SignalR (real-time communication), Serilog.

### Frontend

Angular 21 single-page application using SignalR for real-time updates. Run from `src/Teams.Web/`.

### Design

Responsive UI designs in `designs/teams.pen` covering three breakpoints:

| Breakpoint | Width | Navigation |
|---|---|---|
| Mobile | 390px | Bottom tab bar |
| Tablet | 768px | Top navigation bar |
| Desktop | 1440px | Sidebar navigation |

Screens: Home Dashboard, Chat, Meetings, Team Members.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (LTS)
- npm 10+

## Getting Started

### Backend

```bash
dotnet restore
dotnet build
dotnet run --project src/Teams.ApiGateway
```

The API gateway starts and hosts all services. Swagger UI is available at `/swagger`.

### Frontend

```bash
cd src/Teams.Web
npm install
npm start
```

The dev server runs at `http://localhost:4200` by default.

### Running Tests

```bash
# Backend
dotnet test

# Frontend
cd src/Teams.Web
npm test
```

## Project Structure

| Path | Description |
|---|---|
| `src/Teams.ServiceDefaults/` | Shared config: auth, logging, EF Core, MassTransit setup |
| `src/Teams.Contracts/` | Event contracts and DTOs shared across services |
| `src/Teams.Web/projects/` | Angular libraries and app source |
| `designs/teams.pen` | UI mockups for all breakpoints |

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for development setup, branch strategy, and code standards.

## License

Proprietary. All rights reserved.
