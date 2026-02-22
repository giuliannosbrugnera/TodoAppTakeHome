# TodoAppTakeHome API

Minimal ASP.NET Core Web API for managing tasks.

---

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- FluentValidation
- Swagger (Swashbuckle)
- CSharpier

---

## Project Structure

```
TodoAppTakeHome.Api/
 ├── Controllers/
 ├── Services/
 ├── Entities/
 ├── DTOs/
 ├── Validators/
 ├── Data/
 ├── Migrations/
 └── Program.cs
```

### Architecture Notes

- Thin controllers
- Business logic encapsulated in services
- Validation via FluentValidation
- EF Core with committed migrations
- Environment-aware configuration
- Entities are not exposed directly through the API (DTO separation)

---

## Prerequisites

- .NET 10 SDK

Install required tools:

```bash
dotnet tool install -g dotnet-ef
dotnet tool install -g csharpier
```

---

## Database Setup

Apply migrations:

```bash
dotnet ef database update -p TodoAppTakeHome.Api
```

Migrations are committed to source control to ensure reproducible schema.
SQLite database files (`*.db`) are ignored.

---

## Running Tests

The solution uses **xUnit** and **EF Core InMemory** for isolated unit tests.

### From terminal

```bash
dotnet restore
dotnet test TodoAppTakeHome.Tests/TodoAppTakeHome.Tests.csproj
```

---

## Run the Application

```bash
dotnet run --project TodoAppTakeHome.Api/TodoAppTakeHome.Api.csproj
```

Default URL:

```
http://localhost:5274
```

Swagger (Development only):

```
http://localhost:5274/swagger
```

---

## API Endpoints

Base route:

```
/api/tasks
```

| Method | Route           | Description    |
| ------ | --------------- | -------------- |
| GET    | /api/tasks      | Get all tasks  |
| GET    | /api/tasks/{id} | Get task by id |
| POST   | /api/tasks      | Create task    |
| PUT    | /api/tasks/{id} | Update task    |
| DELETE | /api/tasks/{id} | Delete task    |

---

## Formatting

Run:

```bash
dotnet csharpier format .
```

---

## Notes

- HTTPS redirection enabled outside Development.
- Development-only reset endpoint is isolated and excluded from production.
