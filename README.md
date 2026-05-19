# BloggingPlatform (.NET 8, Clean Architecture)

## Architecture
- `BloggingPlatform.Domain`: Entities, enums, domain contracts.
- `BloggingPlatform.Application`: DTOs, service contracts/implementations, validators, mapping.
- `BloggingPlatform.Infrastructure`: EF Core DbContext, repository, JWT, file storage, seed data.
- `BloggingPlatform.API`: Controllers, middleware, DI, auth, swagger.
- `BloggingPlatform.Tests`: Unit/integration test samples.

## Prerequisites
- .NET SDK 8.0+
- SQL Server

## Setup
1. Create solution/projects:
```bash
# optional if you generate from scratch
dotnet new sln -n BloggingPlatform
```
2. Update `BloggingPlatform.API/appsettings.json` connection string + JWT secret.
3. Apply migrations:
```bash
dotnet ef migrations add InitialCreate -p BloggingPlatform.Infrastructure -s BloggingPlatform.API
dotnet ef database update -p BloggingPlatform.Infrastructure -s BloggingPlatform.API
```
4. Run:
```bash
dotnet run --project BloggingPlatform.API
```
5. Swagger:
- `https://localhost:5001/swagger`

## Seeded data
- Roles: `Admin`, `Author`, `User`
- Admin account:
  - username: `admin`
  - email: `admin@blog.local`
  - password: `Admin@123`

## Response format
```json
{
  "success": true,
  "message": "string",
  "data": {},
  "errors": []
}
```

## Notes
- Passwords hashed with ASP.NET Core `IPasswordHasher<User>`.
- Refresh tokens are persisted in DB.
- Includes upload service for `jpg/png/webp` with size validation.
