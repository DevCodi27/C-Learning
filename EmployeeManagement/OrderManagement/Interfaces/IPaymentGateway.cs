using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Interfaces;

/// <summary>
/// Contract for payment processing.
/// Allows swapping payment providers (Stripe, PayPal, Mock) without touching business logic.
/// </summary>
public interface IPaymentGateway
{
    Task<bool> ChargeAsync(decimal amount, string customerId, CancellationToken cancellationToken = default);
}
