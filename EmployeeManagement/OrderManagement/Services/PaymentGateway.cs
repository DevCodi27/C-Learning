using EmployeeManagement.OrderManagement.Interfaces;

namespace EmployeeManagement.OrderManagement.Services;

/// <summary>
/// Simulated Payment Gateway.
/// Demonstrates non-blocking I/O using Task.Delay instead of thread-blocking Thread.Sleep.
/// </summary>
public class StripePaymentGateway : IPaymentGateway
{
    public async Task<bool> ChargeAsync(decimal amount, string customerId, CancellationToken cancellationToken = default)
    {
        Console.WriteLine($"[PaymentGateway] Contacting Stripe for customer '{customerId}', amount: ${amount:F2}...");

        // Simulate network latency (500ms) without blocking any OS thread!
        // The thread returns to the .NET ThreadPool while waiting.
        await Task.Delay(500, cancellationToken);

        // Simulate business rule: orders over $10,000 need manual approval / decline
        if (amount > 10_000m)
        {
            Console.WriteLine("[PaymentGateway] ❌ Charge failed: Amount exceeds credit limit ($10,000).");
            return false;
        }

        Console.WriteLine($"[PaymentGateway] ✅ Successfully charged ${amount:F2}.");
        return true;
    }
}
