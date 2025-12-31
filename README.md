# Todo App (Dapr + Docker Compose)

A learning application demonstrating:
- Dapr (service invocation, pub/sub)
- Docker Compose
- ASP.NET Web API
- Entity Framework Core + PostgreSQL

## Architecture

The application consists of two services:

- **Manager**
  - Public HTTP API
  - Validates input data
  - Uses Dapr to communicate with other services

- **Accessor**
  - Works with the database (PostgreSQL)
  - Processes events from the queue
  - Returns data on request from Manager

Interaction:
- `POST /todos` → async via Dapr pub/sub
- `GET /todos/{id}` → sync via Dapr service invocation

---

## Technologies Used

- .NET 10
- ASP.NET Web API
- Dapr
- Docker / Docker Compose
- PostgreSQL
- Redis
- Entity Framework Core

---

## Requirements

- Docker Desktop
- Docker Compose (v2)
- Free ports on the host:
  - `5084` — Manager API
  - `5228` — Accessor API
  - `5432` — PostgreSQL
  - `6379` — Redis

---

## Running the Application

From the solution root:

```bash
docker compose up --build
