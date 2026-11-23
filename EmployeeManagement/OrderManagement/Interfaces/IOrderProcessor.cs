using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Interfaces;

public record OrderProcessingResult(bool IsSuccess, string Message, Order Order);

/// <summary>
/// Orchestration contract for the entire order workflow.
/// </summary>
public interface IOrderProcessor
{
    Task<OrderProcessingResult> ProcessOrderAsync(Order order, CancellationToken cancellationToken = default);
}
