---
name: csharp-skill-manager
description: >-
  Rules and procedures for dynamically creating, structuring, and updating C# and .NET topic skills.
  Used by csharp-trainer, csharp-teacher, csharp-tester, csharp-reviewer, and csharp-clarifier to ensure
  knowledge is permanently captured and referenced during learning cycles.
---

# C# Training Skill Management System

This skill defines how knowledge skills are created, formatted, and dynamically updated during the training cycle.

## Skill Storage Convention
- All topic skills are stored in: `.agents/skills/<topic-slug>/SKILL.md`
- Example topics:
  - `csharp-fundamentals` (Day 1: Value vs Ref types, strings, arrays, exceptions)
  - `csharp-oop-solid` (Day 2: OOP, interfaces, records, structs, SOLID)
  - `csharp-linq-collections` (Day 3: Collections, delegates, LINQ, deferred execution)
  - `csharp-async-testing` (Day 4: xUnit, AAA, Task/async/await, CancellationToken, CLI)
  - `csharp-gate1-checkpoint` (Day 5: Gate 1 mini-build, regression testing)
  - `aspnet-rest-controllers` (Day 6: HTTP verbs, controllers, DTOs, routing, Swagger)
  - `aspnet-di-middleware` (Day 7: DI lifetimes, middleware pipeline, config, ILogger)
  - `aspnet-layered-architecture` (Day 8: Clean architecture, validation, error handling)
  - `efcore-data-persistence` (Day 9: DbContext, migrations, tracking, relationships)
  - `aspnet-security-jwt` (Day 10: JWT, auth vs auth, claims, roles, Gate 2)

## Standard Topic Skill Format
Every topic skill must follow this structure:

```markdown
---
name: <topic-slug>
description: <Concise 1-2 sentence description of the topic>
---

# <Topic Title>

## 1. Core Concepts & Enterprise Patterns
- Core theory and definition
- Modern C# 12 / .NET 8+ syntax and patterns
- Idiomatic code samples (Employee & Department domain)

## 2. Common Pitfalls & Anti-Patterns
- What to avoid in production
- Naive vs Enterprise comparison

## 3. Interview Questions & Evaluation Criteria
- Must-know interview questions with model answers
- Edge cases and evaluation rubric

## 4. Trainee Doubts & Clarifications (Dynamically Updated)
- Questions asked by the trainee and their comprehensive answers
- Memory layout / CLR internal details

## 5. Code Review Findings & Best Practices (Dynamically Updated)
- Specific feedback and improvements identified during code reviews
```

## Dynamic Update Protocol
1. **Teaching Phase**: When introducing a new topic, create the skill if it doesn't exist. Add new syntax patterns or tips discovered during the lesson.
2. **Clarification Phase**: Whenever the trainee asks a question or has a misconception, append the question and clarified explanation to Section 4.
3. **Review Phase**: Whenever a code review discovers an anti-pattern or refactoring opportunity, log it in Section 5 so the trainee can review it later.
4. **Testing Phase**: Add tricky edge cases or interview follow-ups to Section 3.
