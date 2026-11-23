namespace EmployeeManagement.OrderManagement.Models;

/// <summary>
/// Immutable representation of an item inside an order.
/// Using record for value-based equality and immutability.
/// </summary>
public record OrderItem(string ProductId, string ProductName, int Quantity, decimal UnitPrice)
{
    public decimal SubTotal => Quantity * UnitPrice;
}
