using EmployeeManagement.OrderManagement.Interfaces;
using EmployeeManagement.OrderManagement.Models;
using EmployeeManagement.OrderManagement.Services;

namespace EmployeeManagement.OrderManagement;

/// <summary>
/// Guided Demo class showing:
/// 1. Decoupled Interface wiring via Dependency Injection
/// 2. Asynchronous execution (Task vs Thread)
/// 3. High-concurrency batch processing via Task.WhenAll
/// 4. Timeout cancellation handling via CancellationTokenSource
/// </summary>
public static class OrderDemo
{
    public static async Task RunDemoAsync()
    {
        Console.WriteLine("\n=======================================================");
        Console.WriteLine("    ENTERPRISE ORDER MANAGEMENT DEMO (INTERFACES & ASYNC)");
        Console.WriteLine("=======================================================");

        // 1. Compose decoupled dependencies (Clean Architecture / DI)
        IInventoryService inventory = new InMemoryInventoryService();
        IPaymentGateway payment = new StripePaymentGateway();
        INotificationService notification = new EmailNotificationService();

        IOrderProcessor processor = new OrderProcessor(inventory, payment, notification);

        // 2. Sample Orders
        var order1 = new Order
        {
            CustomerId = "CUST-001",
            CustomerEmail = "alice@example.com",
            Items = new List<OrderItem>
            {
                new("PROD-01", "Developer Laptop", 1, 1500m),
                new("PROD-02", "Mechanical Keyboard", 1, 150m)
            }
        };

        var order2 = new Order
        {
            CustomerId = "CUST-002",
            CustomerEmail = "bob@example.com",
            Items = new List<OrderItem>
            {
                new("PROD-03", "4K Monitor", 1, 400m) // Out of stock!
            }
        };

        var order3 = new Order
        {
            CustomerId = "CUST-003",
            CustomerEmail = "charlie@vip.com",
            Items = new List<OrderItem>
            {
                new("PROD-01", "Enterprise Server Cluster", 8, 12000m) // Exceeds $10k payment limit!
            }
        };

        // --- Demo A: Sequential Processing with Business Rules ---
        Console.WriteLine("\n--- DEMO A: Processing Successful & Boundary Orders ---");
        await processor.ProcessOrderAsync(order1);
        await processor.ProcessOrderAsync(order2);
        await processor.ProcessOrderAsync(order3);

        // --- Demo B: High-Throughput Concurrency with Task.WhenAll ---
        Console.WriteLine("\n--- DEMO B: Concurrent Batch Processing (Task.WhenAll) ---");
        Console.WriteLine("Spawning multiple orders concurrently over the ThreadPool...");

        var batchOrders = new List<Order>
        {
            new() { CustomerId = "BATCH-1", CustomerEmail = "user1@demo.com", Items = new() { new("PROD-01", "Laptop", 1, 1500m) } },
            new() { CustomerId = "BATCH-2", CustomerEmail = "user2@demo.com", Items = new() { new("PROD-02", "Keyboard", 1, 150m) } }
        };

        // Instead of processing one by one, execute concurrently:
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var tasks = batchOrders.Select(order => processor.ProcessOrderAsync(order));
        var results = await Task.WhenAll(tasks);
        stopwatch.Stop();

        Console.WriteLine($"\n[Batch Summary] Completed {results.Length} orders concurrently in {stopwatch.ElapsedMilliseconds}ms.");

        // --- Demo C: Cooperative Cancellation & Timeout ---
        Console.WriteLine("\n--- DEMO C: Cancellation Token Timeout Simulation ---");
        using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(100)); // Super strict 100ms timeout

        var timeoutOrder = new Order
        {
            CustomerId = "CUST-TIMEOUT",
            CustomerEmail = "timeout@user.com",
            Items = new List<OrderItem> { new("PROD-01", "Laptop", 1, 1500m) }
        };

        await processor.ProcessOrderAsync(timeoutOrder, cts.Token);
    }
}
