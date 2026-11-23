namespace EmployeeManagement.OrderManagement.Models;

public class Order
{
    public Guid OrderId { get; init; } = Guid.NewGuid();
    public string CustomerId { get; init; } = string.Empty;
    public string CustomerEmail { get; init; } = string.Empty;
    public List<OrderItem> Items { get; init; } = new();
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    public decimal TotalAmount => Items.Sum(item => item.SubTotal);
}
