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

### Virtualization & Devirtualization (CLR Internals)
- **Virtual Method**: A method marked with `virtual` (or `abstract`) whose target implementation is resolved dynamically at runtime based on the actual object type, using the type's **VTable (Virtual Method Table)**.
- **IL Dispatch (`call` vs `callvirt`)**:
  - `call`: Direct CPU jump to an exact method address known at compile time.
  - `callvirt`: Checks for null reference, fetches the object's `MethodTable`, indexes into the `VTable`, and invokes the overridden implementation.
- **`sealed` Keyword**:
  - On a class (`sealed class FullTimeEmployee`): Prohibits inheritance entirely.
  - On an overridden method (`public sealed override void Process()`): Prevents subsequent derived classes from overriding it further.
- **Why use `sealed`?**:
  1. **Domain Security / Invariant Protection**: Guarantees nobody can inherit and subvert core business rules.
  2. **JIT Devirtualization**: When the JIT compiler knows a class is sealed, it proves at runtime that no derived classes exist. It turns an indirect `callvirt` VTable lookup into a direct, blazing-fast `call`, and can even **inline** the method body directly.

---

## 2. Common Pitfalls & Anti-Patterns

- ❌ **Anti-Pattern 1: The "God Base Class" Hierarchy**: Forcing unrelated entities (`Contractor`, `Intern`, `SoftwareBot`) into a single `Employee` inheritance tree, violating LSP and ISP.
- ❌ **Anti-Pattern 2: 1:1 "Header Interfaces"**: Blindly creating an interface for every single class (`IEmployee`, `IAddress`) without polymorphism or decoupling requirements.
- ❌ **Anti-Pattern 3: Leaving Everything Unsealed**: Defaulting to open, unsealed classes without explicit intent, which degrades JIT devirtualization and causes the fragile base class problem.

---

## 3. Interview Questions & Evaluation Rubric

1. **What is the difference between `virtual`, `override`, and `sealed` in C#?**
   - *Model Answer*: `virtual` defines a method in a base class that derived classes can optionally customize. `override` provides the specific subclass implementation. `sealed` terminates the inheritance chain—preventing further classes from inheriting or overriding the method.
2. **What is JIT Devirtualization, and how does `sealed` help performance?**
   - *Model Answer*: When a method is virtual, the CLR must resolve it through the VTable pointer at runtime. When a class or method is marked `sealed`, the JIT compiler knows no other subclass can override it, so it bypasses the VTable lookup and emits a direct call or inlines the method.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)

### Q1: What is `sealed` and virtualization/devirtualization, and why are they used?
- **Virtualization**: The mechanism enabling runtime polymorphism. An object pointer on the heap points to a `MethodTable` with a `VTable`. When a method is `virtual`, the CLR looks up which version to run at runtime based on the concrete instance type.
- **Sealed**: Stops inheritance. Prevents tampering with business logic and allows the JIT compiler to eliminate VTable lookups via **devirtualization**.

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)
- **Architecture Organization**: Separate concerns into dedicated files (`Models/`, `Services/`) instead of accumulating hundreds of lines inside top-level `Program.cs`.
