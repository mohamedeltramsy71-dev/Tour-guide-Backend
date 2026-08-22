using Microsoft.AspNetCore.Identity;
using TourGuide.Application.DTOs.Notifications;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class NotificationService : INotificationService
{
    private readonly IUnitOfWork _uow;
    private readonly INotificationPushService _push;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    public NotificationService(
        IUnitOfWork uow,
        INotificationPushService push,
        IEmailService emailService,
        UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _push = push;
        _emailService = emailService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<NotificationDto>> GetMyNotificationsAsync(string userId, int page, int pageSize)
    {
        var notifications = await _uow.Repository<Notification>()
            .FindAsync(n => n.UserId == userId);

        return notifications
            .OrderBy(n => n.IsRead)
            .ThenByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(MapToDto);
    }

    public async Task MarkAsReadAsync(string userId, int notificationId)
    {
        var notification = await _uow.Repository<Notification>()
            .FindOneAsync(n => n.Id == notificationId && n.UserId == userId)
            ?? throw new NotFoundException("Notification not found.");

        notification.IsRead = true;
        _uow.Repository<Notification>().Update(notification);
        await _uow.SaveChangesAsync();
    }

    public async Task MarkAllAsReadAsync(string userId)
    {
        var notifications = await _uow.Repository<Notification>()
            .FindAsync(n => n.UserId == userId && !n.IsRead);

        foreach (var n in notifications)
        {
            n.IsRead = true;
            _uow.Repository<Notification>().Update(n);
        }

        await _uow.SaveChangesAsync();
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        return await _uow.Repository<Notification>()
            .CountAsync(n => n.UserId == userId && !n.IsRead);
    }

    public async Task CreateNotificationAsync(string userId, string message, NotificationType type, int? bookingId = null)
    {
        // 1. Save to DB
        var notification = new Notification
        {
            UserId = userId,
            Message = message,
            Type = type,
            BookingId = bookingId,
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Repository<Notification>().AddAsync(notification);
        await _uow.SaveChangesAsync();

        var dto = MapToDto(notification);

        // 2. SignalR push
        await _push.PushAsync(userId, dto);

        // 3. Email — fire and forget
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !string.IsNullOrEmpty(user.Email))
        {
            _ = _emailService.SendNotificationEmailAsync(
                user.Email,
                user.FullName ?? "User",
                message,
                type
            );
        }
    }

    private static NotificationDto MapToDto(Notification n) => new()
    {
        Id = n.Id,
        Message = n.Message,
        Type = n.Type.ToString(),
        IsRead = n.IsRead,
        CreatedAt = n.CreatedAt,
        BookingId = n.BookingId
    };
}