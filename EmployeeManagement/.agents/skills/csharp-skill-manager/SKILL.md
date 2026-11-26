---
name: csharp-skill-manager
description: >-
  Rules and procedures for dynamically creating, structuring, and updating C# and .NET topic skills.
  Used by csharp-trainer, csharp-teacher, csharp-tester, csharp-reviewer, and csharp-clarifier to ensure
  knowledge is permanently captured and referenced during learning cycles.
---

# C# Training Skill Management System

This skill defines how knowledge skills are created, formatted, and dynamically updated during the training cycle.

## Core Directive: Full-Depth Completeness & Immediate Synchronization
- **Zero Skeletons Rule**: A topic skill must NEVER be created as a partial outline, shallow bullet list, or empty placeholder. 
- **Full-Depth Requirement**: Every skill must be written with exhaustive depth upon inception:
  1. **In-depth theory and runtime internals** (CLR memory mechanics, Stack vs Managed Heap, VTable/MethodTable dispatch, GC generational lifecycles, IL execution).
  2. **Production-grade C# 12 / .NET 8 code examples** modeling the organization's Employee Management domain.
  3. **Architectural trade-offs & anti-patterns** (Naive vs Enterprise, performance traps, boxing, fragile base classes).
  4. **Senior interview questions with complete model answers**.
  5. **Verbatim trainee doubts & clarifications** appended immediately to Section 4 as they arise.
  6. **Code review findings & rules** appended to Section 5 with every code submission.
- **Dual Workspace Sync**: All topic skills must be maintained and synchronized across both `EmployeeManagement/.agents/skills/<topic-slug>/SKILL.md` and `../.agents/skills/<topic-slug>/SKILL.md`.

## Skill Storage Convention
- All topic skills are stored in: `.agents/skills/<topic-slug>/SKILL.md`
- Core Roadmap Topics:
  - `csharp-fundamentals` (Day 1: CTS Types, Value vs Ref, Nullable, Operators, Flow, Parameters, Strings/StringBuilder, Arrays, Exceptions)
  - `csharp-oop-solid` (Day 2: Classes, Primary Constructors, Properties, Structs vs Records, Abstract vs Interfaces, SOLID, Devirtualization)
  - `csharp-linq-collections` (Day 3: Generic Collections, Delegates, Lambdas, LINQ Operators, Deferred Execution, Extension Methods)
  - `csharp-async-testing` (Day 4: xUnit AAA, Facts/Theories, Task/async/await, CancellationToken, CLI Tooling)
  - `csharp-gate1-checkpoint` (Day 5: Gate 1 Mini-Build, Injected Defect Regression, Walkthrough Rubric)
  - `aspnet-rest-controllers` (Day 6: HTTP Verbs, Controllers, DTOs, Routing, OpenAPI/Swagger)
  - `aspnet-di-middleware` (Day 7: DI Lifetimes, Captive Dependencies, Middleware Pipeline, ILogger)
  - `aspnet-layered-architecture` (Day 8: Clean Layering, FluentValidation, Global ProblemDetails)
  - `efcore-data-persistence` (Day 9: DbContext, Migrations, Fluent API, Tracking vs NoTracking, N+1 Prevention)
  - `aspnet-security-jwt` (Day 10: JWT Tokens, Claims, Policies, Secret Management, Gate 2)

## Standard Topic Skill Format
Every topic skill must follow this exact 5-section structure:

```markdown
---
name: <topic-slug>
description: <Concise 1-2 sentence description of the topic>
---

# <Topic Title>

## 1. Core Concepts & Enterprise Patterns
- Complete theory, runtime mechanics (Stack, Heap, CLR)
- Modern C# 12 / .NET 8+ syntax and patterns
- Idiomatic code samples (Employee & Department domain)

## 2. Common Pitfalls & Anti-Patterns
- What to avoid in production
- Naive vs Enterprise comparison

## 3. Interview Questions & Evaluation Criteria
- Must-know senior interview questions with thorough model answers
- Edge cases and evaluation rubric

## 4. Trainee Doubts & Clarifications (Dynamically Updated)
- Specific questions asked by the trainee with comprehensive answers
- Memory layout / CLR internal details

## 5. Code Review Findings & Best Practices (Dynamically Updated)
- Specific feedback and improvements identified during code reviews
```

## Dynamic Update Protocol
1. **Teaching Phase**: When introducing a new topic, create the skill with full-depth content immediately.
2. **Clarification Phase**: Whenever the trainee asks a question or has a doubt, immediately append the question and clarified technical explanation to Section 4.
3. **Review Phase**: Whenever a code review discovers an anti-pattern or refactoring opportunity, log it in Section 5 with actionable best practices.
4. **Testing Phase**: Add tricky edge cases or interview follow-ups to Section 3.
