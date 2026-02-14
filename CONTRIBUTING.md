# Contributing to TeamSync

## Development Setup

1. Clone the repository
2. Install prerequisites: .NET 8 SDK, Node.js (LTS), npm 10+
3. Restore dependencies:
   ```bash
   dotnet restore
   cd src/Teams.Web && npm install
   ```

## Branch Strategy

- `main` - production-ready code
- `develop` - integration branch
- Feature branches: `feature/<short-description>`
- Bug fixes: `fix/<short-description>`

Create branches from `develop` and open pull requests back into `develop`.

## Code Standards

### Backend (.NET)

- Follow existing patterns in `TeamSync.ServiceDefaults` for cross-cutting concerns
- Use MediatR for all request handling (commands and queries)
- Use MassTransit for async inter-service communication
- Place shared DTOs and event contracts in `TeamSync.Contracts`
- Keep services independent; services should communicate only through the message bus or API gateway
- Use nullable reference types (`#nullable enable` is project-wide)

### Frontend (Angular)

- Follow the [Angular style guide](https://angular.dev/style-guide)
- Use Prettier for formatting (config in `package.json`)
- Single quotes, 100-character print width

### Design

- Design files use the Pencil `.pen` format in `designs/`
- Use design tokens (variables) for colors, not hardcoded hex values
- Maintain all three breakpoints (mobile 390px, tablet 768px, desktop 1440px) when adding screens
- Reuse the shared navigation components (desktop sidebar, tablet top nav) for new screens

## Making Changes

1. Create a feature branch from `develop`
2. Make your changes with clear, focused commits
3. Ensure all tests pass:
   ```bash
   dotnet test
   cd src/Teams.Web && npm test
   ```
4. Open a pull request with:
   - Summary of what changed and why
   - Screenshots for UI changes
   - Test coverage for new logic

## Adding a New Microservice

1. Create the project: `dotnet new webapi -n TeamSync.Services.YourService -o src/TeamSync.Services.YourService`
2. Add a reference to `TeamSync.ServiceDefaults`
3. Add the project to `TeamSync.sln`
4. Register the service in the API gateway
5. Add shared contracts to `TeamSync.Contracts` if other services need them

## Adding a New Screen

1. Design all three breakpoints in `designs/teams.pen` (mobile, tablet, desktop)
2. Add the Angular route and component in `src/Teams.Web`
3. Add the navigation entry to the shared nav components in the design file
4. Update any backend services or API endpoints as needed

## Commit Messages

Write concise commit messages that explain **why** the change was made. Use imperative mood:

- `Add meeting recurrence support`
- `Fix timezone offset calculation for DST`
- `Remove deprecated notification endpoint`

## Reporting Issues

Open an issue with:
- Steps to reproduce
- Expected vs actual behavior
- Browser/OS if frontend-related
- Service logs if backend-related
