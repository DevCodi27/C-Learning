using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Interfaces;

/// <summary>
/// Contract for inventory management.
/// Verifies stock availability and reserves items.
/// </summary>
public interface IInventoryService
{
    Task<bool> ReserveStockAsync(IEnumerable<OrderItem> items, CancellationToken cancellationToken = default);
    Task ReleaseStockAsync(IEnumerable<OrderItem> items, CancellationToken cancellationToken = default);
}
