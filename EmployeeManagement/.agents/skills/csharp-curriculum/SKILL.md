---
name: csharp-curriculum
description: >-
  Complete 2-Week C# and ASP.NET Core Web API training syllabus roadmap with daily modules, learning outcomes,
  interview questions, LeetCode challenges, gate criteria, and Capstone requirements.
---

# 2-Week C# and ASP.NET Core Web API Training Curriculum

## Program Overview
- **Domain**: Employee & Department Management System
- **Pacing**: 4–5 hours per training day (5 days Week 1, 5 days Week 2)
- **Checkpoints**: Gate 1 (Day 5) & Gate 2 (Day 10) — 70% knowledge check + working code + test suite

---

## Week 1: C# and .NET Foundations

### Day 1: C# Fundamentals
- **Core Topics**: Variables, types, operators, control flow; Value vs Reference types; Nullable value & reference types; Methods, parameters (`ref`, `out`, `in`, `params`), strings vs `StringBuilder`, arrays; Exceptions & structured error handling.
- **Hands-on Task**: Console application processing employee records with methods & collections.
- **LeetCode**: *Concatenation of Array* (Easy).
- **Interview Prep**: Value vs reference types; string vs StringBuilder; nullable reference types.

### Day 2: Object-Oriented C#
- **Core Topics**: Classes, objects, constructors, properties (init-only, expression-bodied); Encapsulation, inheritance, polymorphism; Interfaces vs Abstract classes; Access modifiers; Records (`record class`, `record struct`) vs Structs; SOLID principles & Composition over inheritance.
- **Hands-on Task**: Domain models & interfaces for Employee/Department with at least 3 classes and 1 interface implementation.
- **LeetCode**: *Number of Steps to Reduce a Number to Zero* (Easy).
- **Interview Prep**: Interface vs abstract class; composition vs inheritance; SOLID with real-world examples.

### Day 3: Modern C# and LINQ
- **Core Topics**: Collections (`List<T>`, `Dictionary<TKey, TValue>`, `HashSet<T>`); Delegates, `Func<T>`, `Action<T>`, Lambdas, Events; LINQ operators (`Where`, `Select`, `GroupBy`, `OrderBy`, `Any`, `All`, `First/FirstOrDefault`); Deferred execution vs Immediate execution (`ToList()`); Extension methods; Pattern matching.
- **Hands-on Task**: Implement in-memory filtering, grouping, sorting, and searching over employee records (at least 8 distinct LINQ queries).
- **LeetCode**: *Contains Duplicate* (Easy).
- **Interview Prep**: `IEnumerable` vs `IQueryable`; deferred execution; `List` vs `Dictionary` performance & hash collisions.

### Day 4: Unit Testing, Async C#, and .NET Tooling
- **Core Topics**: Unit testing boundaries & philosophy; xUnit framework, assertions, Arrange-Act-Assert (AAA); Test naming conventions and edge-case testing; `Task`, `Task<T>`, `async`/`await`, `CancellationToken`, `Task.WhenAll`; CLI tooling (`dotnet new`, `dotnet build`, `dotnet test`, `dotnet run`, NuGet).
- **Hands-on Task**: Multi-project solution (Console App, Class Library, xUnit Test Project) with >= 5 passing tests, plus an async method with cancellation token handling.
- **LeetCode**: *Best Time to Buy and Sell Stock* (Easy).
- **Interview Prep**: Characteristics of a good unit test; AAA pattern; Task vs Thread; restore/build/test lifecycle.

### Day 5: C# Checkpoint & Mini-Build (Gate 1)
- **Goal**: Validate language mastery, clean OOP architecture, and automated test suite.
- **Tasks**: Structured Employee Management C# application, unit tests for boundary/invalid inputs, fix an injected defect with a regression test, live code walkthrough.
- **LeetCode**: *Valid Anagram* (Easy).
- **Gate 1 Evaluation**: 70% threshold on knowledge check + passing automated tests + code walkthrough without notes.

---

## Week 2: ASP.NET Core Web API

### Day 6: ASP.NET Core & REST API Basics
- **Core Topics**: HTTP request/response lifecycle, REST conventions; Controllers, routing (`[Route]`, `[HttpGet]`, etc.), action methods; Model binding (`[FromBody]`, `[FromQuery]`, `[FromRoute]`), DTOs, JSON serialization; HTTP verbs and status codes (200, 201, 204, 400, 404, 500); Swagger / OpenAPI documentation, `.http` files.
- **Hands-on Task**: Runnable Web API with CRUD endpoints for Employees & Departments, documented with OpenAPI.
- **Interview Prep**: Request lifecycle in ASP.NET Core; PUT vs PATCH; when to return 201, 400, 404, or 204.

### Day 7: Dependency Injection & Middleware
- **Core Topics**: Built-in IoC container (`IServiceCollection`, `IServiceProvider`); Service lifetimes: `Transient`, `Scoped`, `Singleton`; Captive dependency pitfalls; Middleware pipeline & ordering (`UseRouting`, `UseAuthentication`, `UseAuthorization`, `UseEndpoints`); Configuration & environment settings (`appsettings.json`, environment variables); Structured logging with `ILogger<T>`.
- **Hands-on Task**: Register services with correct lifetimes, implement custom logging middleware, configure environment settings.
- **Interview Prep**: Why use DI?; What is a captive dependency?; Why does middleware order matter?

### Day 8: Layered Architecture & Validation
- **Core Topics**: Controller vs Service vs Repository responsibilities; DTOs vs Domain Entities & mapping; Separation of concerns & SOLID; Request validation (Data Annotations / FluentValidation); Centralized error handling (`UseExceptionHandler` / `ProblemDetails`).
- **Hands-on Task**: Refactor API into layers (API, Application/Core, Infrastructure/Data), implement validation and consistent error responses.
- **Interview Prep**: Repository vs Service; DTO vs Entity; Middleware vs Action Filters.

### Day 9: EF Core & Database Integration
- **Core Topics**: `DbContext`, `DbSet<T>`, Entity configurations (Fluent API); Relationships (1-1, 1-many) and EF Core migrations (`dotnet ef migrations add`, `dotnet ef database update`); LINQ to Entities (SQL translation); Tracking vs `AsNoTracking()`; Eager loading (`.Include()`), explicit loading; N+1 problem and pagination.
- **Hands-on Task**: Relational database integration (SQLite/SQL Server), relationships between Employee and Department, filtering, sorting, pagination.
- **Interview Prep**: EF Core vs Dapper; N+1 query problem causes and solutions; tracking vs no-tracking query performance.

### Day 10: API Checkpoint & Security (Gate 2)
- **Core Topics**: Authentication vs Authorization; JWT tokens (header, payload, signature), claims, issuance and validation; Role-based & Policy-based authorization (`[Authorize]`); 401 Unauthorized vs 403 Forbidden; Managing secrets securely (Secret Manager).
- **Hands-on Task**: Secure endpoints with JWT, demonstrate authorized, unauthenticated (401), and forbidden (403) responses with unit tests.
- **Gate 2 Evaluation**: Database-backed, layered, tested, and secured API walkthrough.
