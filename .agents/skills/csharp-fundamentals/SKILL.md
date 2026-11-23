---
name: csharp-fundamentals
description: >-
  Day 1 Core Topic Skill: C# syntax, type system (Value vs Reference types, Nullable types),
  methods, parameter modifiers, strings vs StringBuilder, arrays, and exception handling.
---

# Day 1: C# Fundamentals & Type System

## 1. Core Concepts & Enterprise Patterns

### Value Types vs Reference Types
- **Value Types** (`struct`, `int`, `double`, `bool`, `enum`, `DateTime`):
  - Stored directly where declared (typically on the Stack, or inline inside objects on the Heap).
  - Copied by value (modifying a copy does not affect the original).
- **Reference Types** (`class`, `interface`, `record class`, `string`, `object`, arrays):
  - Stored on the Managed Heap; the variable holds a memory pointer (reference) on the stack.
  - Copied by reference (two variables can point to the exact same object instance).

```csharp
// Example: Value vs Reference demonstration
public struct Point { public int X; public int Y; } // Value type
public class Employee { public string Name { get; set; } = string.Empty; } // Reference type

Point p1 = new Point { X = 10, Y = 20 };
Point p2 = p1; // Value copied!
p2.X = 99; // p1.X remains 10

Employee e1 = new Employee { Name = "Alice" };
Employee e2 = e1; // Reference copied!
e2.Name = "Bob"; // e1.Name is now "Bob"
```

### Nullable Types
- **Nullable Value Types**: `int?` is syntactic sugar for `Nullable<int>`. Has `.HasValue` and `.Value`.
- **Nullable Reference Types (C# 8+)**: Enabled via `<Nullable>enable</Nullable>` in `.csproj`.
  - `string` = non-nullable (compiler warns if assigned null).
  - `string?` = explicitly nullable.
  - Null-forgiving operator: `!`. Null-coalescing: `??`, `??=`. Null-conditional: `?.`.

### Strings vs StringBuilder
- `System.String` is **immutable**. Every concatenation (`+`, `+=`) allocates a new string object on the Heap.
- In loops or high-frequency string manipulation, use `System.Text.StringBuilder` to avoid GC pressure.
- Modern C# also offers string interpolation (`$"{employee.Name}"`) which Roslyn compiles efficiently using `DefaultInterpolatedStringHandler`.

### Parameter Modifiers
- `ref`: Passes variable by reference (must be initialized before passing).
- `out`: Passes variable by reference for output (must be assigned before method exits).
- `in`: Passes by reference as read-only (prevents copying large value types without allowing mutation).
- `params`: Allows passing variable number of arguments as an array.

---

## 2. Common Pitfalls & Anti-Patterns
- ❌ **Modifying mutable reference types unknowingly**: Passing an object to a method and having it mutate state unexpectedly. Favor immutable designs or records.
- ❌ **String concatenation in loops**: `for (int i = 0; i < 1000; i++) str += i;` generates 1,000 garbage objects.
- ❌ **Swallowing exceptions**: `catch (Exception) { /* empty */ }` hides critical bugs. Always log or handle specifically.
- ❌ **`catch (Exception ex) { throw ex; }`**: Resets the stack trace! Always use `throw;` to preserve the original stack trace.

---

## 3. Interview Questions & Evaluation Rubric
1. **Explain Value Types vs Reference Types in terms of memory allocation (Stack vs Heap).**
   - *Model Answer*: Value types contain their data directly, allocated on the stack (when local variables) or inline in the enclosing type. Reference types allocate memory on the managed heap, with a pointer reference stored on the stack.
2. **What is the difference between `string` and `StringBuilder`? When would you use which?**
   - *Model Answer*: Strings are immutable; each modification creates a new heap object. StringBuilder is a mutable buffer that expands dynamically, ideal for loops and heavy concatenations.
3. **What is the purpose of Nullable Reference Types if references could always be null in C#?**
   - *Model Answer*: It is a compile-time static analysis feature to prevent `NullReferenceException` by forcing developers to be explicit about nullability intents (`string` vs `string?`).

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)
*(This section will be populated as questions arise during training)*

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)
*(This section will be populated as trainee submits Day 1 code)*
