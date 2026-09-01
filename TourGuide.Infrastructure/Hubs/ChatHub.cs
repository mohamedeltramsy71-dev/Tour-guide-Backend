using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Infrastructure.Data;

namespace TourGuide.Infrastructure.Hubs;

[Authorize]
public class ChatHub : Hub
{
    private readonly AppDbContext _context;
    private readonly IEmailService _emailService;
    private readonly UserManager<ApplicationUser> _userManager;

    private static readonly Dictionary<string, HashSet<string>> _onlineUsers = new();
    private static readonly object _lock = new();

    public ChatHub(
        AppDbContext context,
        IEmailService emailService,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _emailService = emailService;
        _userManager = userManager;
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier!;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

        lock (_lock)
        {
            if (!_onlineUsers.ContainsKey(userId))
                _onlineUsers[userId] = new HashSet<string>();
            _onlineUsers[userId].Add(Context.ConnectionId);
        }

        // ✅ ابعت للناس التانيين إنه اتصل
        await Clients.Others.SendAsync("UserOnline", userId);

        // ✅ ابعتله هو قائمة المتصلين دلوقتي عشان يعرف مين Online
        List<string> onlineUserIds;
        lock (_lock)
        {
            onlineUserIds = _onlineUsers.Keys.ToList();
        }
        await Clients.Caller.SendAsync("OnlineUsersList", onlineUserIds);

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier!;

        lock (_lock)
        {
            if (_onlineUsers.ContainsKey(userId))
            {
                _onlineUsers[userId].Remove(Context.ConnectionId);
                if (_onlineUsers[userId].Count == 0)
                    _onlineUsers.Remove(userId);
            }
        }

        await Clients.Others.SendAsync("UserOffline", userId);
        await base.OnDisconnectedAsync(exception);
    }

    public async Task JoinBookingGroup(int bookingId)
    {
        var userId = Context.UserIdentifier!;

        var booking = await _context.Bookings
            .Include(b => b.GuideProfile)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking is null) return;

        var isParticipant = booking.TouristId == userId ||
                            booking.GuideProfile.UserId == userId;
        if (!isParticipant) return;

        await Groups.AddToGroupAsync(Context.ConnectionId, $"booking_{bookingId}");
    }

    public async Task SendMessage(string receiverId, string content, int bookingId)
    {
        var senderId = Context.UserIdentifier!;

        var booking = await _context.Bookings
            .Include(b => b.GuideProfile)
            .FirstOrDefaultAsync(b => b.Id == bookingId);

        if (booking is null) return;

        var isParticipant = booking.TouristId == senderId ||
                            booking.GuideProfile.UserId == senderId;
        if (!isParticipant) return;

        var message = new Message
        {
            SenderId = senderId,
            BookingId = bookingId,
            Content = content,
            CreatedAt = DateTime.UtcNow,
            IsRead = false
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        var sender = await _context.Users.FindAsync(senderId);

        var dto = new
        {
            message.Id,
            message.Content,
            message.SenderId,
            SenderName = sender?.FullName ?? string.Empty,
            message.BookingId,
            CreatedAt = message.CreatedAt.ToString("o"),
            message.IsRead
        };

        await Clients.Group($"booking_{bookingId}").SendAsync("ReceiveMessage", dto);
        await Clients.Group($"user_{receiverId}").SendAsync("ReceiveMessage", dto);
        await Clients.Caller.SendAsync("ReceiveMessage", dto);

        var isReceiverOnline = false;
        lock (_lock)
        {
            isReceiverOnline = _onlineUsers.ContainsKey(receiverId) &&
                               _onlineUsers[receiverId].Count > 0;
        }

        if (!isReceiverOnline)
        {
            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver != null && !string.IsNullOrEmpty(receiver.Email))
            {
                _ = _emailService.SendNewMessageEmailAsync(
                    receiver.Email,
                    receiver.FullName ?? "User",
                    sender?.FullName ?? "Someone",
                    content.Length > 100 ? content[..100] + "..." : content
                );
            }
        }
    }

    public async Task MarkAsRead(int messageId)
    {
        var message = await _context.Messages.FindAsync(messageId);
        if (message is null) return;

        message.IsRead = true;
        await _context.SaveChangesAsync();

        if (!string.IsNullOrEmpty(message.SenderId))
        {
            await Clients.Group($"user_{message.SenderId}")
                .SendAsync("MessageRead", messageId);
        }
    }
}