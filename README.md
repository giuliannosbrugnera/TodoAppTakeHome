# TodoAppTakeHome – Full Stack Task Management App

Minimal full-stack task management application built with ASP.NET Core and Vue 3.

---

# Backend – TodoAppTakeHome API

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- FluentValidation
- Swagger (Swashbuckle)
- CSharpier
- xUnit (tests)
- EF Core InMemory (test isolation)

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
- Entities are not exposed directly (DTO separation)
- Centralized error handling
- Clean separation of concerns

---

## Prerequisites

- .NET 10 SDK
- Node.js 18+

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

## Running Backend Tests

```bash
dotnet restore
dotnet test TodoAppTakeHome.Tests/TodoAppTakeHome.Tests.csproj
```

---

## Run Backend

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

# Frontend – Vue 3 Application

## Tech Stack

- Vue 3 (Composition API)
- TypeScript
- Vite
- TailwindCSS
- Vitest
- Testing Library

---

## Features

- Create task
- Edit task
- Delete task
- Status update (Todo, InProgress, Done)
- Optional due date
- Filtering by status
- Sorting by due date (asc/desc)
- Per-action loading states
- Centralized animated error component
- Unit tests with mocked API layer

---

## Frontend Architecture

- Parent-owned state (`TaskList`)
- Event-driven child components (`TaskForm`, `TaskItem`)
- Reusable composables (`useDate`, `useTaskStatus`)
- Centralized error display
- Clean component separation
- Mocked API layer for frontend unit tests

---

## Run Frontend

```bash
cd frontend
npm install
npm run dev
```

Default URL:

```
http://localhost:5173
```

---

## Run Frontend Tests

```bash
npm run test
```

---

## Formatting

Backend:

```bash
dotnet csharpier format .
```

---

## Notes

- HTTPS redirection enabled outside Development.
- Development-only reset endpoint is isolated and excluded from production.
- Migrations are committed for reproducible schema.
- DTO separation prevents leaking domain entities.
- Frontend tests mock API calls to ensure deterministic behavior.
- The project is structured for clarity and maintainability, with production considerations in mind.

## Assumptions & Trade-offs

- Authentication and authorization are out of scope.
- Pagination is not implemented due to small dataset assumption.
- State updates occur after successful API responses to ensure consistency.
- SQLite was chosen for simplicity; production would use a managed SQL instance.
- Frontend uses local state; global state management (e.g., Pinia) is not required at current scale.

## Future Improvements

- Add pagination and search.
- Add authentication and role-based access.
- Add integration tests for full-stack flows.
- Add CI pipeline.
- Add caching layer.
- Add optimistic UI updates.
- Add Docker support.
