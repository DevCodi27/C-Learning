---
name: csharp-oop-solid
description: >-
  Day 2 Core Topic Skill: Object-Oriented Programming (OOP) in modern C# 12 / .NET 8,
  deep architectural contrast between Abstract Classes and Interfaces, SOLID principles,
  Template Method vs Strategy patterns, Default Interface Methods, and Static Abstract Members.
---

# Day 2: Object-Oriented Programming & SOLID Principles

## 1. Core Concepts & Enterprise Patterns

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

### Enterprise Domain Model Example (Employee & Department System)

```csharp
namespace EmployeeManagement.Domain;

/// <summary>
/// Capability contract: any entity capable of generating a structured payroll compensation calculation.
/// Decoupled from internal employee identity (can apply to internal staff, 1099 contractors, external consultants).
/// </summary>
public interface IPayrollCalculable
{
    decimal CalculateMonthlyPayout();
    
    // Modern C# 8+ Default Interface Method (DIM) for non-breaking API evolution
    string GetCompensationFrequency() => "Monthly";
}

/// <summary>
/// C# 11+ Static Abstract Member Contract for generic object instantiation / factories
/// </summary>
public interface IEntityFactory<out TSelf> where TSelf : IEntityFactory<TSelf>
{
    static abstract TSelf Create(int id, string name);
}

/// <summary>
/// Behavioral contract for work auditing & reporting
/// </summary>
public interface IWorkReportable
{
    Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Abstract Base Class: Defines identity, invariants, encapsulated state, and the Template Method workflow.
/// Single inheritance root for core organization personnel.
/// </summary>
public abstract class Employee : IPayrollCalculable, IWorkReportable
{
    private string _name = string.Empty;

    // Encapsulated state & invariants
    public int Id { get; init; }

    public string Name
    {
        get => _name;
        init
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);
            _name = value;
        }
    }

    public string Department { get; protected set; }
    public decimal BaseSalary { get; protected set; }
    public DateTime HireDate { get; init; } = DateTime.UtcNow;

    // Constructor enforces invariants across all derived types
    protected Employee(int id, string name, department, decimal baseSalary)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Employee ID must be positive.");
        if (baseSalary < 0) throw new ArgumentOutOfRangeException(nameof(baseSalary), "Base salary cannot be negative.");

        Id = id;
        Name = name;
        Department = department;
        BaseSalary = baseSalary;
    }

    // --- TEMPLATE METHOD PATTERN ---
    // Enforces the standard payroll audit algorithm while letting derived classes supply custom bonuses/deductions.
    public (decimal Gross, decimal Deductions, decimal NetPayout) GeneratePayStub()
    {
        decimal gross = CalculateMonthlyPayout();
        decimal deductions = CalculateStandardTaxDeductions(gross);
        decimal net = gross - deductions;

        LogPayStubGeneration(gross, net);
        return (gross, deductions, net);
    }

    // Abstract hook: Derived classes MUST provide specific payout logic
    public abstract decimal CalculateMonthlyPayout();

    // Virtual hook: Derived classes MAY override default tax deduction algorithm
    protected virtual decimal CalculateStandardTaxDeductions(decimal grossAmount)
    {
        return grossAmount * 0.20m; // Default 20% tax baseline
    }

    private void LogPayStubGeneration(decimal gross, decimal net)
    {
        // Internal shared infrastructure hook
    }

    public abstract Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Concrete Implementation: Full-Time Employee
/// Inherits state & template method from Employee; implements specific calculations.
/// </summary>
public sealed class FullTimeEmployee : Employee
{
    public decimal AnnualBonusPercentage { get; init; }

    public FullTimeEmployee(int id, string name, string department, decimal baseSalary, decimal annualBonusPercentage)
        : base(id, name, department, baseSalary)
    {
        AnnualBonusPercentage = annualBonusPercentage;
    }

    public override decimal CalculateMonthlyPayout()
    {
        decimal monthlyBonus = (BaseSalary * (AnnualBonusPercentage / 100m)) / 12m;
        return BaseSalary + monthlyBonus;
    }

    public override Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"[FTE: {Name}] Logged 40 standard corporate hours in {Department}.");
    }
}

/// <summary>
/// External Contractor: NOT an Employee! Has no HireDate, no internal Department benefits, no BaseSalary.
/// Demonstrates why Single Inheritance fails and Interfaces provide orthogonal capability.
/// </summary>
public sealed class ExternalContractor : IPayrollCalculable, IWorkReportable
{
    public string VendorFirmName { get; init; }
    public decimal HourlyBillingRate { get; init; }
    public int HoursBilledThisMonth { get; private set; }

    public ExternalContractor(string vendorFirmName, decimal hourlyRate)
    {
        VendorFirmName = vendorFirmName;
        HourlyBillingRate = hourlyRate;
    }

    public void RecordHours(int hours) => HoursBilledThisMonth += hours;

    // Implements IPayrollCalculable without inheriting from Employee
    public decimal CalculateMonthlyPayout() => HoursBilledThisMonth * HourlyBillingRate;

    public string GetCompensationFrequency() => "Bi-Weekly Invoiced";

    public Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult($"[Vendor: {VendorFirmName}] Delivered {HoursBilledThisMonth} contracted billing hours.");
    }
}
```

---

## 2. Common Pitfalls & Anti-Patterns

### ❌ Anti-Pattern 1: The "God Base Class" / Fragile Base Class Hierarchy
- **Symptom**: Trying to force entities with different lifecycles (e.g. `ExternalContractor`, `Intern`, `SoftwareBot`) into an `Employee` inheritance tree.
- **Problem**: Subclasses inherit methods and state that make no semantic sense (e.g., `Contractor` having `AnnualLeaveBalance` or `W2TaxWithholding`), violating the **Liskov Substitution Principle (LSP)** and **Interface Segregation Principle (ISP)**.
- **Enterprise Solution**: Model core identity with an abstract base class only where a true, deep "Is-A" invariant exists. Model capabilities and integrations through lightweight, focused interfaces.

### ❌ Anti-Pattern 2: 1:1 "Header Interfaces" Everywhere Without Intent
- **Symptom**: Creating `IEmployee`, `IAddress`, `IPerformanceReview` for every single concrete domain model without any polymorphism, mocking requirement, or behavioral contract.
- **Problem**: Code bloat and maintenance overhead with zero abstraction value.
- **Enterprise Solution**: Interfaces should represent *roles*, *capabilities*, or *seams for dependency injection* (`IPayrollProcessor`, `IEmployeeRepository`). Domain data models / records do not need 1:1 interfaces.

### ❌ Anti-Pattern 3: Abusing Default Interface Methods (DIMs) as Multiple Inheritance
- **Symptom**: Writing heavy stateful business logic inside interfaces using DIMs and casting tricks.
- **Problem**: Interfaces cannot hold backing instance fields. DIMs are designed for backwards-compatible API evolution in libraries (e.g., adding an optional overload to `IEnumerable<T>`), not for general object state management.

---

## 3. Interview Questions & Evaluation Rubric

### Q1: What is the primary architectural difference between an interface and an abstract class in modern C#?
- **Model Answer**: 
  - An **abstract class** models an *"Is-A"* identity hierarchy, maintains protected state/backing fields, enforces object invariants via constructors, and can implement the Template Method pattern using private/protected virtual hooks. A class can only inherit from one abstract class.
  - An **interface** models a *"Can-Do"* capability or behavioral contract with no instance state. Classes and structs can implement multiple interfaces, enabling loose coupling, polymorphic interchangeability, and clean dependency injection.

### Q2: Modern C# allows Default Interface Methods (DIMs). Does this make abstract classes obsolete?
- **Model Answer**:
  - No. Interfaces **cannot hold instance fields or backing state**, cannot have instance constructors to enforce invariant initialization, and cannot have protected members accessible by derived instance fields. 
  - Furthermore, DIMs use explicit interface implementation semantics; if a class implements an interface with a DIM, that method is only callable when the instance is cast to the interface reference, not directly on the concrete class type.
  - Abstract classes remain the sole vehicle for shared object state, encapsulated fields, and lifecycle constructors.

### Q3: What are C# 11+ Static Abstract Members in Interfaces and what problem do they solve?
- **Model Answer**:
  - They allow interfaces to declare static contracts (methods, operators, properties) that implementing classes must provide statically (`static abstract T Create(...)`, `static abstract T operator +(T a, T b)`).
  - This solved generic math in .NET 7/8 (`INumber<T>`) and allows true static polymorphic factory patterns without reflection or runtime instantiation hacks.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)

### Trainee Doubt: "What is difference between interface and abstract clas?"
- **Clarification**:
  1. **Mental Model**: If two things share the same *fundamental identity* and *state* (e.g., `FullTimeEmployee` and `PartTimeEmployee` both have IDs, tax withholdings, and base contracts), use an `abstract class Employee`. If two distinct things merely share a *behavior* (e.g., an `Employee` and an `ExternalVendor` can both calculate a payout), define an `interface IPayrollCalculable`.
  2. **CLR Execution Details**:
     - Abstract class virtual calls resolve via a standard **VTable (Virtual Method Table)** offset lookup.
     - Interface method calls resolve via **IMT (Interface Method Table)** or stub dispatch stubs, which evaluate the target type's interface map.
     - Structs implementing interfaces will undergo **boxing** on the managed heap if cast to the interface type, causing GC allocations unless constrained via generics (`where T : IPayrollCalculable`).

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)

- **Rule 1: Prefer Composition & Interfaces over Deep Inheritance**: Keep class inheritance hierarchies to at most 1–2 levels deep. Use interfaces to compose orthogonal behaviors.
- **Rule 2: Seal Concrete Classes**: Mark leaf implementations as `sealed` (e.g., `public sealed class FullTimeEmployee : Employee`). This aids JIT devirtualization and prevents fragile subclassing.
- **Rule 3: Enforce Invariants in Abstract Constructors**: Never expose public setters on abstract class base state; use `init` or `protected set` and validate in the `protected` constructor.
