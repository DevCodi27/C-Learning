using EmployeeManagement.OrderManagement.Interfaces;
using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Services;

/// <summary>
/// Simulated Email Notification Service.
/// Runs asynchronously to avoid blocking the main order flow.
/// </summary>
public class EmailNotificationService : INotificationService
{
    public async Task SendConfirmationAsync(Order order, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[NotificationService] Sending order confirmation email to {order.CustomerEmail}...");
        await Task.Delay(300, cancellationToken);
        Console.WriteLine($"[NotificationService] 📧 Email delivered for Order #{order.OrderId.ToString()[..8]}.");
    }

    public async Task SendFailureNoticeAsync(Order order, string reason, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[NotificationService] Sending failure alert to {order.CustomerEmail} (Reason: {reason})...");
        await Task.Delay(200, cancellationToken);
        Console.WriteLine($"[NotificationService] ⚠️ Failure notification sent.");
    }
}
