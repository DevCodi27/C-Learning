---
name: csharp-fundamentals
description: >-
  Day 1 Core Topic Skill: C# syntax, variables, CTS type system (Value vs Reference types, Nullable types),
  operators, control flow (guard clauses, switch expressions), methods, parameter modifiers (ref, out, in, params),
  strings vs StringBuilder, arrays & collection expressions, and structured exception handling.
---

# Day 1: C# Fundamentals & Type System

## 1. Core Concepts & Enterprise Patterns

### Variables, Declarations, and Scopes
- **Strongly Typed**: Every variable has an explicit compile-time type known to Roslyn.
- **Type Inference (`var`)**:
  - Compiler infers the exact type at compile time; it is **not** dynamic or loosely typed.
  - *Enterprise rule*: Use `var` when the type is obvious from the right-hand side (e.g., `var emp = new Employee();`), but use explicit types when the type is not immediately clear (e.g., `decimal bonus = CalculateBonus();`).
- **Constants vs Readonly**:
  - `const`: Evaluated at compile-time, must be initialized on declaration, implicitly static. Value baked into calling assemblies.
  - `readonly`: Evaluated at runtime, can be assigned at declaration or in a constructor. Ideal for dependency injection or runtime configuration.

### CTS Types & Memory Semantics
- **Integral Types**: `byte` (8-bit), `short` (16-bit), `int` (32-bit), `long` (64-bit).
- **Floating Point vs Financial Decimal**:
  - `float` (32-bit binary float), `double` (64-bit binary float): Fast, hardware-accelerated, but have base-2 rounding errors (e.g., `0.1 + 0.2 != 0.3`). Never use for money!
  - `decimal` (128-bit high-precision base-10): Exactly represents decimal fractions. **Mandatory for salaries, tax calculations, and payroll in enterprise systems.**
- **Value Types vs Reference Types**:
  - **Value Types** (`struct`, `int`, `decimal`, `bool`, `enum`, `DateTime`):
    - Stored directly where allocated (stack for locals, inline within object payload on heap).
    - Copied by value on assignment or method argument passing.
  - **Reference Types** (`class`, `interface`, `record class`, `string`, `object`, arrays):
    - Stored on the Managed Heap; the variable holds an 8-byte pointer (64-bit CLR) on the stack.
    - Copied by reference; multiple pointers refer to the exact same heap memory address.
- **Boxing & Unboxing**:
  - *Boxing*: Converting a value type to `object` or an interface it implements. Allocates an object header (MethodTable pointer + SyncBlockIndex) on the Heap, copies the value into it. Induces GC pressure.
  - *Unboxing*: Explicitly casting the boxed heap object back to its exact value type. Checks type identity before copying value back to the stack.
- **Type Conversion & Safe Parsing**:
  - *Implicit*: Safe widening conversions without data loss (e.g., `int` -> `long`).
  - *Explicit Casting*: Potential narrowing or truncation (e.g., `(int)3.99m` -> `3`).
  - *Safe Parsing*: Avoid `int.Parse()` on user/external input. Always use `int.TryParse(input, out int result)` to prevent throwing unhandled `FormatException`.

### Nullable Types (Value & Reference)
- **Nullable Value Types**: `decimal?` is syntactic sugar for `Nullable<decimal>`. Uses `.HasValue` and `.Value`.
- **Nullable Reference Types (C# 8+)**: Enabled via `<Nullable>enable</Nullable>` in `.csproj`.
  - `string` = non-nullable (compiler warns if assigned null).
  - `string?` = explicitly nullable.
  - Null-forgiving operator: `!`.
  - Null-coalescing: `??`, `??=`.
  - Null-conditional: `?.` (short-circuits property/method chains).

### Operators
- **Arithmetic**: `+`, `-`, `*`, `/`, `%`. Division between integers truncates decimals (`5 / 2 == 2`). Use `5.0m / 2` to obtain `2.5m`.
- **Relational & Equality**: `==`, `!=`, `<`, `>`, `<=`, `>=`.
  - For value types: compares underlying bit/value fields.
  - For reference types (default): compares memory addresses (identity), except `string` which overrides `==` for character sequence equality.
- **Logical**: Short-circuiting `&&`, `||` vs eager `&`, `|`.
- **Ternary Operator**: `condition ? trueExpression : falseExpression`.
- **Pattern Matching Operators**:
  - `is`: `if (employee is Manager m && m.ReportsCount > 5)`
  - `as`: Safe reference cast that returns `null` instead of throwing `InvalidCastException`.

### Control Flow & Clean Architecture Idioms
- **Guard Clauses over Nested If-Else**:
  - Prevent "Arrow Anti-Pattern" (pyramid of doom). Validate preconditions early and exit/return immediately.
- **Modern Switch Expressions (C# 8+)**:
  - Replaces verbose procedural `switch-case-break` blocks with clean, functional pattern matching expressions.
  - Supports relational patterns (`<`, `>`, `>=`), tuple patterns, and property patterns.
- **Looping Constructs**:
  - `for`: Index-based iteration, minimal overhead, supports forward/backward stepping.
  - `foreach`: Operates on any type implementing `IEnumerable<T>` (or with a compliant `GetEnumerator()` method). Iteration variable is read-only.
  - `while` & `do-while`: Condition-based execution. `do-while` guarantees at least one execution pass.
- **Loop Controls**:
  - `break`: Terminate the innermost loop immediately.
  - `continue`: Skip the remainder of the current iteration and evaluate the loop condition again.

### Methods & Parameter Modifiers (`ref`, `out`, `in`, `params`)
- **Default Passing (By Value)**: Value types pass bitwise copies; reference types pass pointer copies.
- **`ref` (Two-way Reference)**:
  - Variable must be initialized before passing.
  - Method reads and mutates the caller's original memory storage.
- **`out` (Output Reference)**:
  - Variable does not need prior initialization.
  - Method **must** assign a value before returning. Enforces the `TryParse` pattern.
- **`in` (Read-only Reference)**:
  - Variable is passed by reference to avoid copying large stack structures (e.g. large `readonly struct`), but compiler forbids mutation.
- **`params` (Variadic Arguments)**:
  - Accepts zero, one, or multiple comma-separated arguments, bundling them as an array. In C# 12, can also bind to collections like `ReadOnlySpan<T>`.

### Strings vs. StringBuilder
- **`System.String` Immutability**:
  - Every string concatenation (`+`, `+=`) allocates a brand-new string on the Managed Heap.
  - In high-frequency loops, this triggers thousands of garbage allocations and frequent Gen 0 GC pauses.
- **`System.Text.StringBuilder`**:
  - Mutable internal buffer (`char[]`) that expands dynamically.
  - Ideal for loops, tabular report generation, and complex string assembly.
- **String Interpolation Handler (C# 10+)**:
  - Modern interpolated strings (`$"{emp.Name}"`) compile to `DefaultInterpolatedStringHandler` which minimizes allocations for standard one-off prints.

### Arrays & Modern C# 12 Collection Expressions
- **Collection Expressions (`[...]`)**: Modern concise syntax for initializing arrays (`string[] depts = ["HR", "IT", "Finance"];`).
- **Indices & Ranges**:
  - `^1` represents the last element (1 from the end).
  - `1..3` slices a sub-array from index 1 up to (exclusive) index 3.
- **Rectangular (`[,]`) vs Jagged (`[][]`) Arrays**:
  - Rectangular arrays (`decimal[,]`) allocate a single contiguous block of memory with fixed dimensions.
  - Jagged arrays (`int[][]`) are arrays of arrays, allowing rows of varying lengths and better cache locality in many CLR scenarios.

### Structured Exception Handling
- **Exception Anatomy**: `try-catch-finally`. `finally` always runs (even on return/error) for resource cleanup.
- **Exception Filters (`when`)**: Catch exceptions conditionally without unwinding the stack (e.g., `catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)`).
- **Rethrowing Mechanics**:
  - ❌ `throw ex;`: Resets the stack trace to the catch line, erasing the original failure line number.
  - ✅ `throw;`: Preserves the complete original stack trace and line numbers.
- **Custom Exceptions**: Derive from `System.Exception`, provide standard constructors, and capture domain context (e.g., `EmployeeId`).

---

## 2. Common Pitfalls & Anti-Patterns

- ❌ **Using `double` or `float` for Currency/Payroll**: Causes floating-point inaccuracy due to binary IEEE 754 representation. Always use `decimal`.
- ❌ **Deep Arrow Anti-Pattern (Nested Ifs)**: Writing nested conditionals 5 levels deep instead of using guard clauses to exit early.
- ❌ **Modifying Collections inside `foreach`**: Calling `list.Remove()` or `list.Add()` during `foreach` iteration throws `InvalidOperationException` because the collection enumerator version is invalidated.
- ❌ **Unsafe Casting with `(Type)` instead of `is` or `as`**: Directly casting unknown objects causes unhandled `InvalidCastException`.
- ❌ **String Concatenation in Loops**: Doing `str += line` in loops allocates thousands of intermediate string objects on the Heap. Use `StringBuilder`.
- ❌ **`catch (Exception ex) { throw ex; }`**: Resets the stack trace and destroys production debugging info. Always use `throw;`.
- ❌ **Hidden Boxing in String Formatting**: Passing value types to legacy APIs like `string.Format("{0}", 42)` boxes `int` into `object`. Modern string interpolation (`$"{42}"`) avoids this via Roslyn handlers.

---

## 3. Interview Questions & Evaluation Rubric

1. **Why should `decimal` always be chosen over `double` or `float` for financial calculations in an Employee Payroll system?**
   - *Model Answer*: `float` and `double` are base-2 binary floating-point numbers which cannot accurately represent fractional decimals like 0.1, leading to rounding discrepancies. `decimal` is a 128-bit base-10 fixed-point type with 28-29 digits of precision, ensuring exact representations of currency.
2. **What happens during Boxing and Unboxing under the CLR hood, and why does it affect performance?**
   - *Model Answer*: Boxing takes a value type from the stack, allocates a wrapper object on the managed heap with an object header and method table pointer, and copies the value into it. Unboxing verifies the type pointer and copies the bits back to the stack. Boxing causes extra heap allocations and triggers garbage collector (GC) runs.
3. **What is the difference between `const` and `readonly` in C#?**
   - *Model Answer*: `const` is a compile-time constant, implicitly static, and its literal value is hardcoded into calling assemblies at compile time. `readonly` can be assigned at runtime (such as in a constructor), allowing dynamic initialization based on configuration or runtime parameters.
4. **How do Guard Clauses improve readability over nested `if-else` statements?**
   - *Model Answer*: Guard clauses validate edge cases and invalid conditions at the top of a method, returning or throwing early. This eliminates deep indentation ("Arrow Code") and allows the happy path to flow cleanly without nested brackets.
5. **What is the difference between `ref`, `out`, and `in` parameter modifiers?**
   - *Model Answer*: `ref` passes an already initialized variable by reference for read/write access. `out` passes an uninitialized variable that must be assigned before the method returns. `in` passes a variable by reference as read-only to avoid copying large value types.
6. **What is the critical difference between `throw;` and `throw ex;`?**
   - *Model Answer*: `throw;` preserves the original stack trace and source line numbers where the error actually occurred. `throw ex;` resets the stack trace to the catch block, hiding the true origin of the bug.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)

### Q1: What does the `m` suffix mean (e.g. `100.50m`)?
- In C#, literal numbers with decimal points (like `10.5`) default to `double` (64-bit binary float).
- The `m` or `M` suffix explicitly tells the compiler that the literal is of type `System.Decimal` (128-bit base-10 fixed-point representation).
- Why `m`? `d` was already reserved for `double`. Think of `m` as **M**onetary / **M**oney.
- Without `m`, writing `decimal salary = 100.50;` fails compilation because C# prohibits implicit conversion from `double` to `decimal` to protect against silent precision loss.

### Q2: What is the Roslyn Compiler?
- **Roslyn** is the official open-source compiler platform for C# and Visual Basic (`Microsoft.CodeAnalysis`), written in C# itself.
- Unlike legacy native C++ compilers that acted as "black boxes" (Source Code In -> Binary Out), Roslyn operates as a **Compiler-as-a-Service**.
- It exposes rich APIs for:
  - **Syntax Trees**: Parsing source code into structured tokens and ASTs.
  - **Semantic Model**: Understanding symbol bindings, type resolutions, and scopes.
  - **Diagnostics & Analyzers**: Real-time warnings, code fixes, and IDE IntelliSense.
  - **Source Generators**: Metaprogramming and emitting code at compile-time.
  - **Emit**: Generating IL bytecode and metadata into `.dll` or `.exe` assemblies.

### Q3: What is IL (Intermediate Language)?
- **IL** (Common Intermediate Language, also known as CIL or MSIL) is the low-level, CPU-independent bytecode instruction set that C# code compiles into.
- Instead of compiling directly to Intel x64 or ARM machine code, Roslyn compiles C# into IL packed into portable executable assemblies (`.dll`).
- At runtime, the .NET CLR's **JIT (Just-In-Time) Compiler** converts this IL bytecode into native machine code optimized specifically for the target operating system and host CPU processor.

### Q4: What does "GC Managed" mean?
- **GC (Garbage Collector)** is the automated memory manager in the .NET CLR.
- **Unmanaged** languages (like C/C++) require developers to manually allocate (`malloc`) and free (`free`) memory. Forgetting leads to memory leaks; freeing prematurely causes memory corruption or dangling pointer crashes.
- **Managed Memory** in .NET means the developer instantiates reference types (`new Employee()`) on the Managed Heap, and the runtime tracks object lifetimes.
- When memory pressure occurs, the Garbage Collector identifies objects that are no longer reachable from any "GC Root" (active stack variables, CPU registers, static references) and automatically sweeps/compacts that memory.
- Uses a **Generational Model**:
  - **Gen 0**: Short-lived new allocations (frequent, sub-millisecond sweeps).
  - **Gen 1**: Buffer generation for surviving objects.
  - **Gen 2**: Long-lived objects (e.g., singletons, static caches; sweeping is more expensive).

### Q5: Why do `public` and the method name trigger errors in `Program.cs`?
- When `Program.cs` uses **Top-Level Statements** (without an explicit `class Program { ... }`), any method declared in the file is treated as a **Local Function** of the generated top-level entry point.
- **Local functions cannot have access modifiers** (`public`, `private`, `internal`, `protected`) because they only exist within the local scope of the entry point.
- Placing `public` triggers `CS0106: The modifier 'public' is not valid for this item` and confuses Roslyn's parser on the function name.
- **Fix**: Remove `public`. Use `static decimal MethodName(...)` or simply `decimal MethodName(...)`.

### Q6: Why does `throw ex;` destroy stack traces compared to `throw;`?
- `throw ex;` passes the exception variable back to the CLR as if it were brand new, overwriting the `StackTrace` property to point to the line where `throw ex;` is located.
- `throw;` is a special IL instruction (`rethrow`) that tells the runtime to resume unwinding the stack from the original throw point, preserving the original class, method, and source line number.

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)

### Exercise Review: `CalculateAnnualTax` Implementation
- **Syntax**: Ensure semicolons are placed on the `throw` statement (`throw new ...;`) and not after the closing curly brace of an `if` block.
- **Exception Constructors**: `ArgumentOutOfRangeException(paramName, message)` takes `paramName` first. Using `new ArgumentOutOfRangeException("message")` puts the message in the `ParamName` field. Prefer `ArgumentOutOfRangeException.ThrowIfNegative(grossSalary)` in .NET 8+.
- **Boundary Checks (Negative Tax)**: When applying deductions/credits, always clamp to the lower bound (e.g., `Math.Max(0m, baseTax - credit)` or a ternary check) to prevent negative tax liability.
- **Literal Suffix Consistency**: Always append `m` to financial literals (e.g., `1500m` instead of `1500`) to avoid implicit type coercion.
- **DRY Calculation**: Calculate `decimal baseTax = grossSalary * taxRate;` once before applying conditional discounts, avoiding redundant arithmetic in ternary branches.
- **Top-Level Statements & Local Functions**: In top-level statements, methods are local functions and cannot have `public` modifiers (CS0106).
- **Console Input Parsing**: Check `if (!decimal.TryParse(...))` with negation, not `if (decimal.TryParse(...))`, and exit early via guard clause on invalid input.
- **Boolean Expressions**: Avoid `condition ? true : false`. Simply assign `condition` directly, and prefer case-insensitive string comparison (`string.Equals(str, "yes", StringComparison.OrdinalIgnoreCase)`).
