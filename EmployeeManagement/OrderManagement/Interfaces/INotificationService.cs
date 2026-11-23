using EmployeeManagement.OrderManagement.Models;

namespace EmployeeManagement.OrderManagement.Interfaces;

/// <summary>
/// Contract for sending customer notifications.
/// Can be implemented by Email, SMS, Push notification, or Mock.
/// </summary>
public interface INotificationService
{
    Task SendConfirmationAsync(Order order, CancellationToken cancellationToken = default);
    Task SendFailureNoticeAsync(Order order, string reason, CancellationToken cancellationToken = default);
}
