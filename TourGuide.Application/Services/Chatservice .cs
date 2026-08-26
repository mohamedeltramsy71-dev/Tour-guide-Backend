using TourGuide.Application.DTOs.Chat;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class ChatService : IChatService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChatService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ConversationDto>> GetConversationsAsync(string userId)
    {
        var bookings = await _unitOfWork.Chat.GetBookingsWithMessagesAsync(userId);

        return bookings.Select(b =>
        {
            var isTourist = b.TouristId == userId;
            var otherUser = isTourist ? b.GuideProfile.User : b.Tourist;
            var lastMessage = b.Messages.OrderByDescending(m => m.CreatedAt).First();
            var unread = b.Messages.Count(m => m.SenderId != userId && !m.IsRead);

            return new ConversationDto
            {
                BookingId = b.Id,
                OtherUserId = otherUser.Id,
                OtherUserName = otherUser.FullName,
                OtherUserAvatar = otherUser.AvatarUrl,
                LastMessage = lastMessage.Content,
                LastMessageAt = lastMessage.CreatedAt,
                UnreadCount = unread
            };
        })
        .OrderByDescending(c => c.LastMessageAt)
        .ToList();
    }

    public async Task<IEnumerable<MessageDto>> GetMessagesAsync(int bookingId, string userId, int page, int pageSize)
    {
        var booking = await _unitOfWork.Chat.GetBookingWithGuideAsync(bookingId);

        if (booking is null)
            throw new NotFoundException("Booking not found");

        if (booking.TouristId != userId && booking.GuideProfile.UserId != userId)
            throw new UnauthorizedException("You are not part of this booking");

        var messages = await _unitOfWork.Chat.GetMessagesAsync(bookingId, page, pageSize);

        return messages.Select(m => new MessageDto
        {
            Id = m.Id,
            Content = m.Content,
            IsRead = m.IsRead,
            CreatedAt = m.CreatedAt,
            SenderId = m.SenderId,
            SenderName = m.Sender.FullName,
            BookingId = m.BookingId
        });
    }

    public async Task<int> GetUnreadCountAsync(string userId)
    {
        var bookingIds = (await _unitOfWork.Repository<Booking>()
            .FindAsync(b => b.TouristId == userId || b.GuideProfile.UserId == userId))
            .Select(b => b.Id)
            .ToList();

        return await _unitOfWork.Chat.GetUnreadCountAsync(bookingIds, userId);
    }

    public async Task MarkMessagesAsReadAsync(int bookingId, string userId)
    {
        var messages = await _unitOfWork.Repository<Message>()
            .FindAsync(m => m.BookingId == bookingId && m.SenderId != userId && !m.IsRead);

        foreach (var message in messages)
        {
            message.IsRead = true;
            _unitOfWork.Repository<Message>().Update(message);
        }

        await _unitOfWork.SaveChangesAsync();
    }
}