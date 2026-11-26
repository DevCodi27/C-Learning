---
name: csharp-oop-solid
description: >-
  Day 2 Core Topic Skill: Object-Oriented Programming (OOP) in modern C# 12 / .NET 8,
  Classes, Primary Constructors, Properties (init-only, expression-bodied), Classes vs Structs vs Records,
  deep architectural contrast between Abstract Classes and Interfaces, SOLID principles,
  Template Method pattern, Virtualization, and Sealed Devirtualization.
---

# Day 2: Object-Oriented Programming & SOLID Principles

## 1. Core Concepts & Enterprise Patterns

### Classes, Properties, and Constructors (C# 12)

#### Modern Property Accessors
- `get`: Reads the value.
- `set`: Allows mutation at any time throughout the object's lifecycle.
- `init` (Init-Only Setter, C# 9+): Can only be assigned during object initialization (via constructor or object initializer `new Employee { Id = 1 }`). Once constructed, the property is **immutable**.
- `=>` (Expression-Bodied Property): Calculates and returns a value on-the-fly without allocating a backing field.

```csharp
public class Department
{
    // Immutable ID (set once at creation)
    public int Id { get; init; }

    // Read-write property with backing storage
    public string Name { get; set; } = string.Empty;

    // Expression-bodied calculated property
    public string DisplayTag => $"[Dept #{Id}: {Name.ToUpperInvariant()}]";
}
```

#### C# 12 Primary Constructors
Primary constructors allow declaring constructor parameters directly on the class or record header, eliminating repetitive private backing fields and manual assignments.

```csharp
// Modern C# 12 Primary Constructor
public class SalarySlip(int employeeId, decimal grossSalary, decimal deductions)
{
    public int EmployeeId { get; } = employeeId;
    public decimal GrossSalary { get; } = grossSalary;
    public decimal Deductions { get; } = deductions;
    public decimal NetSalary => GrossSalary - Deductions;

    public void Print() => Console.WriteLine($"Net Payout for Emp #{EmployeeId}: {NetSalary:C}");
}
```

#### Enforcing Invariants in Constructors
Enterprise constructors must defend their object state before the instance is constructed:

```csharp
public abstract class BaseEmployee
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal BaseSalary { get; init; }

    protected BaseEmployee(int id, string name, decimal baseSalary)
    {
        // 1. Guard against non-positive ID
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);

        // 2. Guard against empty/whitespace strings
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        // 3. Guard against negative compensation
        ArgumentOutOfRangeException.ThrowIfNegative(baseSalary);

        Id = id;
        Name = name;
        BaseSalary = baseSalary;
    }
}
```

---

### Classes vs. Structs vs. Records: The Complete Matrix

| Feature | `class` | `struct` | `record class` | `record struct` |
| :--- | :--- | :--- | :--- | :--- |
| **CLR Memory Location** | Managed Heap (Reference Type) | Stack / Inline (Value Type) | Managed Heap (Reference Type) | Stack / Inline (Value Type) |
| **Equality Semantics** | Reference identity (`==` checks pointers) | Value equality (field-by-field reflection) | **Value equality** (synthesized by Roslyn) | **Value equality** (efficient without reflection) |
| **Default Mutability** | Mutable | Mutable (unless `readonly struct`) | **Immutable by default** (init-only) | Mutable (unless `readonly record struct`) |
| **`with` Expressions** | ❌ Not supported | ❌ Not supported | ✅ Non-destructive mutation | ✅ Non-destructive mutation |
| **Inheritance** | ✅ Single class inheritance | ❌ No inheritance (interfaces only) | ✅ Inherits other record classes | ❌ No inheritance (interfaces only) |
| **Primary Use Case** | Stateful domain entities, services, controllers | Micro value types (<16 bytes: Point, Guid, DateOnly) | **DTOs, API payloads, domain events, messages** | High-performance, zero-alloc value tokens |

#### The Power of `record`: Value Equality & Non-Destructive Mutation (`with`)
```csharp
// Positional Record declaration:
public record EmployeeDto(int Id, string FullName, decimal Salary);

var emp1 = new EmployeeDto(101, "Alice", 75000m);
var emp2 = new EmployeeDto(101, "Alice", 75000m);

// 1. Value Equality (checks properties, NOT memory pointer)
Console.WriteLine(emp1 == emp2); // Outputs: TRUE!

// 2. Non-destructive Mutation using 'with':
var promotedAlice = emp1 with { Salary = 88000m };
Console.WriteLine(promotedAlice.Salary); // 88000
Console.WriteLine(emp1.Salary);          // 75000 (Original is untouched!)
```

---

### Abstract Classes vs. Interfaces: The Architectural Contrast

| Dimension | Abstract Class | Interface (Modern C# 12 / .NET 8) |
| :--- | :--- | :--- |
| **Philosophical Intent** | **"Is-A"** relationship (Identity & Shared Lineage). Defines what an object *is*. | **"Can-Do"** relationship (Contract & Capability). Defines what an object *can do*. |
| **Inheritance Model** | **Single Class Inheritance**: A class can inherit from only *one* base class (abstract or concrete). | **Multiple Implementation**: A class or struct can implement *multiple* interfaces. |
| **State & Memory** | Can define instance fields, protected backing state, auto-properties with backing storage, and finalizers. Allocated as part of the object layout. | **No instance state**. Cannot declare instance fields or backing fields. Can only declare properties (accessors). |
| **Constructors** | Can declare instance constructors (including C# 12 primary constructors) to enforce mandatory invariant state during instantiation. | **Cannot declare instance constructors**. (Can have static constructors for static state initializing). |
| **Access Modifiers** | Supports `public`, `protected`, `internal`, `protected internal`, `private protected`, and `private` members. | Defaults to `public`. C# 8+ allows `private`, `protected`, `internal`, and `static` members for helper logic. |
| **Method Implementation** | Can provide fully implemented methods, `virtual` methods (with default overrideable logic), or `abstract` methods (requiring derived class override). | Historically pure contracts. C# 8+ supports **Default Interface Methods (DIMs)** with implementation bodies. |
| **Static Abstract Members** | Supported via static methods, but cannot enforce polymorphic static contracts across generic type parameters. | **C# 11+ Static Abstract Members**: Enables static polymorphism, generic math (`INumber<T>`), and static abstract factory methods (`static abstract T Create(...)`). |
| **Runtime Dispatch (CLR)** | Dispatched through standard **vtable (Virtual Method Table)** slots via direct offset index lookup (faster call overhead). | Dispatched through **Interface Method Table (IMT)** or fat interface pointers (slight dispatch overhead / interface stub dispatch). |
| **Value Types (structs)** | Cannot inherit from abstract classes. | Can implement interfaces (note: causes boxing if cast to the interface type). |

---

### Virtualization & Devirtualization (CLR Internals)
- **Virtual Method**: A method marked with `virtual` (or `abstract`) whose target implementation is resolved dynamically at runtime based on the actual object type, using the type's **VTable (Virtual Method Table)**.
- **IL Dispatch (`call` vs `callvirt`)**:
  - `call`: Direct CPU jump to an exact method address known at compile time.
  - `callvirt`: Checks for null reference, fetches the object's `MethodTable`, indexes into the `VTable`, and invokes the overridden implementation.
- **`sealed` Keyword**:
  - On a class (`sealed class FullTimeEmployee`): Prohibits inheritance entirely.
  - On an overridden method (`public sealed override void Process()`): Prevents subsequent derived classes from overriding it further.
- **Why use `sealed`?**:
  1. **Domain Security / Invariant Protection**: Guarantees nobody can inherit and subvert core business rules (fragile base class defense).
  2. **JIT Devirtualization**: When the JIT compiler knows a class is sealed, it proves at runtime that no derived classes exist. It turns an indirect `callvirt` VTable lookup into a direct, blazing-fast `call`, and can even **inline** the method body directly.

---

### SOLID Principles in Enterprise Architecture

#### 1. S — Single Responsibility Principle (SRP)
> *"A class should have one, and only one, reason to change."*
- **Violation**: An `Employee` class that manages its own properties, calculates taxes, connects to SQL Server, and formats PDF reports.
- **Enterprise Solution**:
  - `BaseEmployee` / `FullTimeEmployee`: Owns domain state and invariants.
  - `TaxCalculator` / `PayrollService`: Owns mathematical and tax algorithms.
  - `EmployeeRepository`: Owns database CRUD persistence.

#### 2. O — Open/Closed Principle (OCP)
> *"Software entities should be open for extension, but closed for modification."*
- You should be able to introduce a new compensation type (e.g., `CommissionEmployee` or `Contractor`) without editing or breaking the existing payroll processing loops or switch blocks.
- **Mechanism**: Polymorphism via `IPayrollCalculable`.

#### 3. L — Liskov Substitution Principle (LSP)
> *"Subtypes must be substitutable for their base types without altering program correctness."*
- Subclasses must honor the contract of their parent class.
- **Violation**: If `Contractor` inherits from `BaseEmployee`, but calling `contractor.BaseSalary` throws `NotSupportedException` because contractors don't have base salaries.
- **Enterprise Solution**: `Contractor` should NOT inherit from `BaseEmployee`. Both should independently implement `IPayrollCalculable`.

#### 4. I — Interface Segregation Principle (ISP)
> *"Clients should not be forced to depend on interfaces they do not use."*
- **Violation**: A monolithic `IEmployeeService` with 30 methods (payroll, vacations, performance reviews, badges, IT permissions).
- **Enterprise Solution**: Segregate into granular, focused capability interfaces:
  - `IPayrollCalculable` (contains `CalculateMonthlyPayout()`)
  - `IWorkReportable` (contains `GenerateWeeklySummary()`)
  - `ILeaveRequestable` (contains `SubmitVacationRequest()`)

#### 5. D — Dependency Inversion Principle (DIP)
> *"High-level modules should not depend on low-level modules. Both should depend on abstractions."*
- Business services should never instantiate concrete infrastructure classes (`new SqlDatabaseConnection()`, `new SmtpMailer()`).
- High-level classes depend on injected interfaces (`IEmployeeRepository`, `INotificationService`), which allows swapping implementations, configuring lifetimes in ASP.NET Core DI containers, and unit testing with mocks.

---

### Complete Enterprise Domain Architecture Model

```csharp
namespace EmployeeManagement.Models;

// 1. Interface: Capability Contract
public interface IPayrollCalculable
{
    decimal CalculateMonthlyPayout();
}

// 2. Abstract Base Class: Identity & Invariants
public abstract class BaseEmployee : IPayrollCalculable
{
    public int Id { get; init; }
    public string Name { get; init; }
    public decimal BaseSalary { get; init; }

    protected BaseEmployee(int id, string name, decimal baseSalary)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegative(baseSalary);

        Id = id;
        Name = name;
        BaseSalary = baseSalary;
    }

    // Template Method Pattern: Defines the invariant payroll workflow
    public decimal GenerateMonthlyPaySlip()
    {
        decimal gross = CalculateMonthlyPayout();
        decimal tax = gross * 0.15m;
        return gross - tax;
    }

    public abstract decimal CalculateMonthlyPayout();
}

// 3. Sealed Concrete Subclass (FTE)
public sealed class FullTimeEmployee : BaseEmployee
{
    public decimal BonusPercentage { get; init; }

    public FullTimeEmployee(int id, string name, decimal baseSalary, decimal bonusPercentage)
        : base(id, name, baseSalary)
    {
        BonusPercentage = bonusPercentage;
    }

    public override decimal CalculateMonthlyPayout() =>
        BaseSalary + (BaseSalary * (BonusPercentage / 100m));
}

// 4. Sealed Standalone Class (External Contractor)
public sealed class Contractor : IPayrollCalculable
{
    public string CompanyName { get; init; }
    public decimal HourlyRate { get; init; }
    public int HoursWorked { get; set; }

    public Contractor(string companyName, decimal hourlyRate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(companyName);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(hourlyRate);

        CompanyName = companyName;
        HourlyRate = hourlyRate;
    }

    public decimal CalculateMonthlyPayout() => HourlyRate * HoursWorked;
}
```

---

## 2. Common Pitfalls & Anti-Patterns

- ❌ **Anti-Pattern 1: The "God Base Class" Hierarchy**: Forcing unrelated entities (`Contractor`, `Intern`, `SoftwareBot`) into a single `Employee` inheritance tree, violating LSP and ISP.
- ❌ **Anti-Pattern 2: 1:1 "Header Interfaces"**: Blindly creating an interface for every single class (`IEmployee`, `IAddress`) without polymorphism or decoupling requirements.
- ❌ **Anti-Pattern 3: Leaving Everything Unsealed**: Defaulting to open, unsealed classes without explicit intent, which degrades JIT devirtualization and causes the fragile base class problem.
- ❌ **Anti-Pattern 4: Exposing Mutable Collections**: Exposing `public List<Employee> Employees { get; set; }` allows callers to clear or corrupt internal state. Always expose `IReadOnlyList<T>` or `IEnumerable<T>`.
- ❌ **Anti-Pattern 5: Swallowing Constructor Invariants**: Allowing objects to be constructed with negative IDs, null names, or invalid state, pushing null/range checks throughout the entire codebase.

---

## 3. Interview Questions & Evaluation Rubric

1. **What is the difference between `class`, `struct`, and `record` in modern C#?**
   - *Model Answer*: A `class` is a mutable reference type allocated on the heap comparing identity by reference. A `struct` is a value type allocated on the stack comparing by value. A `record` is a reference or value type synthesized with value-based equality, immutability by default, non-destructive mutation (`with`), and formatted display, ideal for DTOs and API models.
2. **What is the primary architectural difference between an interface and an abstract class in modern C#?**
   - *Model Answer*: An abstract class models an "Is-A" identity hierarchy, maintains protected state/backing fields, enforces object invariants via constructors, and can implement the Template Method pattern. An interface models a "Can-Do" behavioral capability with no instance state, allowing multiple implementations and loose coupling.
3. **What is JIT Devirtualization, and how does `sealed` help performance?**
   - *Model Answer*: When a method is virtual, the CLR must resolve it through the VTable pointer at runtime. When a class or method is marked `sealed`, the JIT compiler knows no other subclass can override it, so it bypasses the VTable lookup and emits a direct call or inlines the method.
4. **How do Primary Constructors in C# 12 improve readability and maintainability?**
   - *Model Answer*: Primary constructors allow parameters to be declared directly in the class or record definition, eliminating the boilerplate of private backing fields and manual assignments while keeping constructors concise.
5. **Explain Liskov Substitution Principle (LSP) with an Employee vs Contractor example.**
   - *Model Answer*: If `Contractor` inherits from `BaseEmployee`, but calling `contractor.BaseSalary` throws an exception because contractors are billed hourly without base salaries, LSP is violated. To adhere to LSP, both classes should implement a common `IPayrollCalculable` interface without inheriting from each other.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)

### Q1: What is `sealed` and virtualization/devirtualization, and why are they used?
- **Virtualization**: The mechanism enabling runtime polymorphism. An object pointer on the heap points to a `MethodTable` with a `VTable`. When a method is `virtual`, the CLR looks up which version to run at runtime based on the concrete instance type.
- **Sealed**: Stops inheritance. Prevents tampering with business logic and allows the JIT compiler to eliminate VTable lookups via **devirtualization**.

### Q2: How should parameter validation errors be thrown in constructors?
- Use `ArgumentOutOfRangeException.ThrowIfNegativeOrZero(id)` for IDs (must be >= 1).
- Use `ArgumentException.ThrowIfNullOrWhiteSpace(name)` for required strings.
- Use `ArgumentOutOfRangeException.ThrowIfNegative(baseSalary)` for amounts that can be 0 but not negative.
- If using explicit `if`, always pass `nameof(param)` first: `throw new ArgumentOutOfRangeException(nameof(id), "Must be positive.");`

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)

- **Architecture Organization**: Separate concerns into dedicated files (`Models/`, `Services/`) instead of accumulating hundreds of lines inside top-level `Program.cs`.
- **Prefer Composition & Interfaces over Deep Inheritance**: Keep class inheritance hierarchies to at most 1–2 levels deep. Use interfaces to compose orthogonal behaviors.
- **Seal Concrete Classes**: Mark leaf implementations as `sealed` (e.g., `public sealed class FullTimeEmployee : BaseEmployee`).
- **Enforce Invariants in Constructors**: Never expose public setters on base domain state; use `init` or `protected set` and validate in the constructor.
