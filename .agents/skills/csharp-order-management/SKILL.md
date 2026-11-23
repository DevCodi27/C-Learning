---
name: csharp-order-management
description: >-
  Core skill for Order Management System: Interface contracts (IOrderProcessor, INotificationService,
  IPaymentGateway), domain models (Order, OrderItem, Customer), and asynchronous processing (Task vs Thread,
  async/await, CancellationToken).
---

# Order Management System: Interfaces & Concurrency (Task vs Thread)

## 1. Core Concepts & Enterprise Patterns

### Interface-Based Architecture
- **Interface Segregation**: Clean contracts defining *what* needs to be done, decoupling consumers from concrete implementations.
- Key contracts for Order Management:
  - `IOrderProcessor`: Orchestrates validation, payment, and inventory deduction.
  - `IPaymentGateway`: External payment simulation.
  - `INotificationService`: Sends async order confirmations (email/SMS).
  - `IOrderRepository`: In-memory or database persistence contract.

```csharp
public interface IOrderProcessor
{
    Task<OrderResult> ProcessOrderAsync(Order order, CancellationToken cancellationToken = default);
}

public interface IPaymentGateway
{
    Task<bool> ChargeAsync(decimal amount, string customerId, CancellationToken cancellationToken = default);
}

public interface INotificationService
{
    Task SendConfirmationAsync(string customerEmail, Guid orderId);
}
```

### Thread vs Task (The Concurrency Model in Modern .NET)
- **`System.Threading.Thread` (Legacy / Low-Level)**:
  - Represents an actual OS thread.
  - Heavyweight: allocates ~1MB stack memory per thread.
  - High context-switching cost.
  - In modern .NET, we almost **never** manually instantiate `new Thread()`.
- **`System.Threading.Tasks.Task` (Modern / TPL)**:
  - Higher-level abstraction managed by the .NET ThreadPool.
  - Lightweight and optimized for asynchronous I/O and CPU parallelism.
  - Scales to tens of thousands of concurrent operations without exhausting OS threads.
- **`async` / `await`**:
  - Non-blocking I/O. When an order waits for a payment gateway or database response, the thread is released back to the ThreadPool.
- **`CancellationToken`**:
  - Cooperative cancellation pattern allowing long-running operations or order processing timeouts to gracefully abort.

---

## 2. Common Pitfalls & Anti-Patterns
- ❌ **Using `Thread.Sleep()` inside async methods**: Blocks the ThreadPool thread! Always use `await Task.Delay(...)`.
- ❌ **Async void**: Never use `async void` except in UI event handlers. In business logic/APIs, use `async Task`.
- ❌ **Calling `.Result` or `.Wait()`**: Causes thread starvation and deadlocks. Always `await`.
- ❌ **Tightly coupling to concrete classes**: `OrderProcessor` instantiating `new PaypalGateway()` directly inside its constructor instead of injecting `IPaymentGateway`.
- ❌ **Ignoring `CancellationToken`**: Failing to propagate tokens down to nested async calls.

---

## 3. Interview Questions & Evaluation Rubric
1. **Why do we prefer `Task` and `async/await` over `System.Threading.Thread`?**
   - *Model Answer*: Threads are heavyweight OS threads (~1MB stack, high context switch overhead). Tasks are lightweight abstractions executed by the .NET ThreadPool. Async/await enables asynchronous I/O without holding onto threads, maximizing throughput.
2. **What is the difference between an Interface and an Abstract Class?**
   - *Model Answer*: An interface defines a contract with no state (multiple interfaces can be implemented). An abstract class can have state (fields), constructors, and partial implementations (single inheritance only).
3. **How does cooperative cancellation work in C#?**
   - *Model Answer*: A `CancellationTokenSource` creates a `CancellationToken`. Long-running tasks periodically check `token.ThrowIfCancellationRequested()` or pass it to I/O methods like `Task.Delay` or `HttpClient`.

---

## 4. Trainee Doubts & Clarifications (Dynamically Updated)
*(Will be updated as trainee asks questions)*

---

## 5. Code Review Findings & Best Practices (Dynamically Updated)
*(Will be updated as code is submitted and reviewed)*
