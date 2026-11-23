namespace EmployeeManagement.OrderManagement.Models;

public enum OrderStatus
{
    Pending,
    InventoryReserved,
    Paid,
    Completed,
    Failed,
    Cancelled
}
