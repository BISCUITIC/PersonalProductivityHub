# Personal Productivity Hub

A backend API built with C# and .NET for managing personal productivity, including projects and tasks.

---

## Features

* User authentication (registration & login)
* Project management
* Task management within projects
* Task prioritization & status tracking
* Clean architecture (Domain / Application / Infrastructure / API)



## Technology  Stack

* C#
* .NET 9
* ASP.NET Core (Minimal API)
* ASP.NET Identity
* Swagger (OpenAPI)
* Entity Framework Core
* Microsoft SQL Server



## Architecture

The project follows **Clean Architecture principles**, separating responsibilities into layers:

```text
Domain          → Core business logic (Entities, Enums, Interfaces)
Application     → Use cases, DTOs, Services
Infrastructure  → Database, Repositories, Identity
API             → Endpoints, configuration, request handling
```

### 🔹 Domain

* Entities: `Project`, `ProjectTask`
* Enums: task priority & status
* Repository interfaces

### 🔹 Application

* Business logic services
* DTOs for data transfer
* Mapping extensions
* Result pattern implementation

### 🔹 Infrastructure

* Entity Framework Core
* Database context & configurations
* Repository implementations
* Identity (users)

### 🔹 API

* Minimal API endpoints
* Authentication endpoints
* Swagger configuration
* Exception handling

## API Endpoints

### Auth

* POST `/auth/register`
* POST `/auth/login`
* POST `/auth/logout`
* GET `/auth/me`

### Projects

* GET `/projects`
* POST `/projects`
* GET `/projects/{id}`
* PUT `/projects/{id}`
* DELETE `/projects/{id}`

### Tasks

* GET `/projects/{projectId}/tasks`
* POST `/projects/{projectId}/tasks`
* GET `/projects/{projectId}/tasks/{taskId}`
* PATCH `/projects/{projectId}/tasks/{taskId}`
* DELETE `/projects/{projectId}/tasks/{taskId}`

---


## Purpose of the Project

This project was created to:

* Practice building scalable backend applications
* Learn clean architecture principles
* Demonstrate real-world .NET development skills

---

## License

MIT License
