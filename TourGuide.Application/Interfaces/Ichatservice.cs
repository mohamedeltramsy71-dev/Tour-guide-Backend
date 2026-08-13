using TourGuide.Application.DTOs.Chat;

namespace TourGuide.Application.Interfaces;

public interface IChatService
{
    Task<IEnumerable<ConversationDto>> GetConversationsAsync(string userId);
    Task<IEnumerable<MessageDto>> GetMessagesAsync(int bookingId, string userId, int page, int pageSize);
    Task<int> GetUnreadCountAsync(string userId);
}