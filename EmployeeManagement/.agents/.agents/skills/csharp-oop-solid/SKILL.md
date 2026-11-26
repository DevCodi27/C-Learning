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

public interface IPayrollCalculable
{
    decimal CalculateMonthlyPayout();
    string GetCompensationFrequency() => "Monthly";
}

public interface IWorkReportable
{
    Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default);
}

public abstract class Employee : IPayrollCalculable, IWorkReportable
{
    private string _name = string.Empty;

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

    protected Employee(int id, string name, string department, decimal baseSalary)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id), "Employee ID must be positive.");
        if (baseSalary < 0) throw new ArgumentOutOfRangeException(nameof(baseSalary), "Base salary cannot be negative.");

        Id = id;
        Name = name;
        Department = department;
        BaseSalary = baseSalary;
    }

    public (decimal Gross, decimal Deductions, decimal NetPayout) GeneratePayStub()
    {
        decimal gross = CalculateMonthlyPayout();
        decimal deductions = CalculateStandardTaxDeductions(gross);
        decimal net = gross - deductions;

        return (gross, deductions, net);
    }

    public abstract decimal CalculateMonthlyPayout();

    protected virtual decimal CalculateStandardTaxDeductions(decimal grossAmount)
    {
        return grossAmount * 0.20m;
    }

    public abstract Task<string> GenerateWeeklySummaryAsync(CancellationToken cancellationToken = default);
}

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
```

---

## 2. Common Pitfalls & Anti-Patterns

- ❌ **Anti-Pattern 1: The "God Base Class" Hierarchy**: Forcing unrelated entities (`Contractor`, `Intern`, `SoftwareBot`) into a single `Employee` inheritance tree, violating LSP and ISP.
- ❌ **Anti-Pattern 2: 1:1 "Header Interfaces"**: Blindly creating an interface for every single class (`IEmployee`, `IAddress`) without polymorphism or decoupling requirements.
- ❌ **Anti-Pattern 3: Abusing DIMs as Multiple Inheritance**: Writing heavy stateful business logic inside interfaces using Default Interface Methods.

---

## 3. Interview Questions & Evaluation Rubric

1. **What is the primary architectural difference between an interface and an abstract class in modern C#?**
   - *Model Answer*: An abstract class models an "Is-A" identity hierarchy, maintains protected state and fields, enforces invariants via constructors, and supports the Template Method pattern. An interface models a "Can-Do" behavioral contract with no instance state, enabling multiple implementations and loose coupling.
2. **Modern C# allows Default Interface Methods (DIMs). Does this make abstract classes obsolete?**
   - *Model Answer*: No. Interfaces cannot hold instance fields or backing state, cannot have instance constructors, and cannot have protected members accessible by derived instance fields.
3. **What are C# 11+ Static Abstract Members in Interfaces?**
   - *Model Answer*: They allow interfaces to declare static contracts (`static abstract T Create(...)`), enabling generic math and static polymorphic factory patterns.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)
*(This section will be populated as questions arise during training)*

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)
*(This section will be populated as trainee submits Day 2 code)*
