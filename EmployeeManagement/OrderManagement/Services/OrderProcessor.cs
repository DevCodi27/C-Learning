using EmployeeManagement.OrderManagement.Interfaces;
using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Services;

/// <summary>
/// High-level orchestrator following SOLID:
/// - Single Responsibility: Only coordinates the order lifecycle.
/// - Open/Closed: New payment or notification providers can be added without modifying this class.
/// - Dependency Inversion: Depends on abstractions (IPaymentGateway, IInventoryService, INotificationService), NOT concrete classes.
/// </summary>
public class OrderProcessor : IOrderProcessor
{
    private readonly IInventoryService _inventory;
    private readonly IPaymentGateway _paymentGateway;
    private readonly INotificationService _notification;

    // Constructor Dependency Injection (DI)
    public OrderProcessor(
        IInventoryService inventory,
        IPaymentGateway paymentGateway,
        INotificationService notification)
    {
        _inventory = inventory;
        _paymentGateway = paymentGateway;
        _notification = notification;
    }

    public async Task<OrderProcessingResult> ProcessOrderAsync(Order order, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"\n=======================================================");
        Console.WriteLine($"[OrderProcessor] Initiating Order #{order.OrderId.ToString()[..8]} for {order.CustomerEmail}...");
        Console.WriteLine($"[OrderProcessor] Total Items: {order.Items.Count}, Total Amount: ${order.TotalAmount:F2}");
        Console.WriteLine($"=======================================================");

        try
        {
            // 1. Cooperative cancellation check
            cancellationToken.ThrowIfCancellationRequested();

            // 2. Step 1: Reserve Inventory
            var stockReserved = await _inventory.ReserveStockAsync(order.Items, cancellationToken);
            if (!stockReserved)
            {
                order.Status = OrderStatus.Failed;
                await _notification.SendFailureNoticeAsync(order, "Items out of stock", cancellationToken);
                return new OrderProcessingResult(false, "Inventory reservation failed", order);
            }
            order.Status = OrderStatus.InventoryReserved;

            // 3. Step 2: Charge Payment
            var paymentSuccess = await _paymentGateway.ChargeAsync(order.TotalAmount, order.CustomerId, cancellationToken);
            if (!paymentSuccess)
            {
                // Compensating transaction: Release stock back if payment fails!
                Console.WriteLine("[OrderProcessor] ⚠️ Payment declined! Reversing inventory reservation...");
                await _inventory.ReleaseStockAsync(order.Items, cancellationToken);
                
                order.Status = OrderStatus.Failed;
                await _notification.SendFailureNoticeAsync(order, "Payment was declined", cancellationToken);
                return new OrderProcessingResult(false, "Payment charge failed", order);
            }
            order.Status = OrderStatus.Paid;

            // 4. Step 3: Complete Order & Send Notification
            order.Status = OrderStatus.Completed;
            await _notification.SendConfirmationAsync(order, cancellationToken);

            Console.WriteLine($"[OrderProcessor] 🎯 Order #{order.OrderId.ToString()[..8]} COMPLETED successfully!");
            return new OrderProcessingResult(true, "Order completed successfully", order);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine($"[OrderProcessor] ⛔ Order #{order.OrderId.ToString()[..8]} was CANCELLED due to timeout or user request.");
            order.Status = OrderStatus.Cancelled;
            // Roll back inventory if it was reserved
            await _inventory.ReleaseStockAsync(order.Items, CancellationToken.None);
            return new OrderProcessingResult(false, "Operation timed out / cancelled", order);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OrderProcessor] 💥 Unexpected system error: {ex.Message}");
            order.Status = OrderStatus.Failed;
            return new OrderProcessingResult(false, $"System failure: {ex.Message}", order);
        }
    }
}
