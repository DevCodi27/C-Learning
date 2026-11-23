using EmployeeManagement.OrderManagement.Interfaces;
using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Services;

/// <summary>
/// In-memory Inventory Service tracking stock levels.
/// </summary>
public class InMemoryInventoryService : IInventoryService
{
    private readonly Dictionary<string, int> _stock = new()
    {
        { "PROD-01", 10 },
        { "PROD-02", 5 },
        { "PROD-03", 0 } // Out of stock
    };

    private readonly Lock _lock = new();

    public async Task<bool> ReserveStockAsync(IEnumerable<OrderItem> items, CancellationToken cancellationToken = default)
    {
        await Task.Delay(200, cancellationToken); // Simulate DB check

        lock (_lock)
        {
            // First check if all items have sufficient quantity
            foreach (var item in items)
            {
                if (!_stock.TryGetValue(item.ProductId, out var available) || available < item.Quantity)
                {
                    Console.WriteLine($"[InventoryService] ❌ Insufficient stock for '{item.ProductName}' (Requested: {item.Quantity}, Available: {available}).");
                    return false;
                }
            }

            // Deduct stock atomically
            foreach (var item in items)
            {
                _stock[item.ProductId] -= item.Quantity;
                Console.WriteLine($"[InventoryService] 📦 Reserved {item.Quantity}x '{item.ProductName}'. Remaining: {_stock[item.ProductId]}.");
            }

            return true;
        }
    }

    public async Task ReleaseStockAsync(IEnumerable<OrderItem> items, CancellationToken cancellationToken = default)
    {
        await Task.Delay(100, cancellationToken);

        lock (_lock)
        {
            foreach (var item in items)
            {
                if (_stock.ContainsKey(item.ProductId))
                {
                    _stock[item.ProductId] += item.Quantity;
                    Console.WriteLine($"[InventoryService] 🔄 Rolled back {item.Quantity}x '{item.ProductName}'. Stock restored to: {_stock[item.ProductId]}.");
                }
            }
        }
    }
}
