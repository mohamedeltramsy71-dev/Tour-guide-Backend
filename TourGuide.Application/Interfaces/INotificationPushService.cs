namespace TourGuide.Application.Interfaces;

public interface INotificationPushService
{
    Task PushAsync(string userId, object payload);
}