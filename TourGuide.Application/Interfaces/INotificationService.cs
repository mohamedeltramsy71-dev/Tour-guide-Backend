using TourGuide.Application.DTOs.Notifications;
using TourGuide.Domain.Enums;

namespace TourGuide.Application.Interfaces;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetMyNotificationsAsync(string userId, int page, int pageSize);
    Task MarkAsReadAsync(string userId, int notificationId);
    Task MarkAllAsReadAsync(string userId);
    Task<int> GetUnreadCountAsync(string userId);
    Task CreateNotificationAsync(string userId, string message, NotificationType type, int? bookingId = null);
}