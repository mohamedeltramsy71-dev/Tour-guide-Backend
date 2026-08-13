using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TourGuide.Domain.Entities;
using TourGuide.Infrastructure.Data;

namespace TourGuide.Infrastructure.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;

    public ChatHub(AppDbContext context)
    {
        _context = context;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier!;

        // إضافة اليوزر لـ group خاص بيه
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

        // إضافة اليوزر لكل booking groups بتاعته
        var bookingIds = await _context.Bookings
            .Where(b => b.TouristId == userId || b.GuideProfile.UserId == userId)
            .Select(b => b.Id)
            .ToListAsync();

        foreach (var bookingId in bookingIds)
            await Groups.AddToGroupAsync(Context.ConnectionId, $"booking_{bookingId}");

        // إخبار الـ group إن اليوزر ده أونلاين
        await Clients.Others.SendAsync("UserOnline", userId);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier!;
        await Clients.Others.SendAsync("UserOffline", userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string receiverId, string content, int bookingId)
    {
        var senderId = Context.UserIdentifier!;

        // save to DB
        var message = new Message
        {
            SenderId = senderId,
            BookingId = bookingId,
            Content = content,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var dto = new
        {
            message.Id,
            message.Content,
            message.SenderId,
            message.BookingId,
            message.CreatedAt,
            message.IsRead
        };

        // إرسال للـ booking group كله
        await Clients.Group($"booking_{bookingId}").SendAsync("ReceiveMessage", dto);
    }

    public async Task MarkAsRead(int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message is null) return;

        message.IsRead = true;
        await _context.SaveChangesAsync();
    }
}