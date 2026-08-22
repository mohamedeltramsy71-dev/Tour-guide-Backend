using Microsoft.AspNetCore.SignalR;
using TourGuide.Application.Interfaces;
using TourGuide.Infrastructure.Hubs;

namespace TourGuide.Infrastructure.Services;

public class NotificationPushService : INotificationPushService
{
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationPushService(IHubContext<NotificationHub> hub)
    {
        _hub = hub;
    }

    public Task PushAsync(string userId, object payload)
        => _hub.Clients.Group($"user_{userId}").SendAsync("NotificationReceived", payload);
}