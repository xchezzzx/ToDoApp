# Todo App - Developer Learning Task

## Overview
Build a simple Todo application demonstrating Dapr, message queues, Docker Compose, EF Core migrations, and testing patterns.

---

## Functional Requirements

1. **Create Todo** - `POST /todos`
   - Accepts: `title` (required), `description` (optional)
   - Manager validates input
   - Manager sends message to queue via Dapr output binding
   - Accessor processes queue message and saves to database

2. **Get Todo** - `GET /todos/{id}`
   - Manager calls Accessor synchronously via Dapr service invocation
   - Accessor retrieves from database and returns

---

## Non-Functional Requirements

### Architecture
- **2 Services (separate dotnet projects)**:
  - **Manager**: HTTP API, input validation, orchestrates via Dapr
  - **Accessor**: Database access (EF Core), queue message processing

### Technologies

1. **Dapr**
   - **Service Invocation** (GET): Manager → Accessor sync call (InvokeMethodAsync)
   - **Output Binding** (POST): Manager → Queue → Accessor async processing (InvokeBindingAsync)
   - Configure components in `dapr/components/`

2. **Entity Framework Core**
   - Use **in-memory database** or PostgreSQL
   - Create initial migration
   - Apply migrations on startup

3. **Docker Compose**
   - Create docker compose project in visual studio
   - Add Manager and Accessor services
   - Include Dapr sidecars
   - should be able to debug from Visual Studio

