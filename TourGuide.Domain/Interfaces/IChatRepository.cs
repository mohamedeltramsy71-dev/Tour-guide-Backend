using TourGuide.Domain.Entities;

namespace TourGuide.Domain.Interfaces;

public interface IChatRepository
{
    Task<IEnumerable<Booking>> GetBookingsWithMessagesAsync(string userId);
    Task<Booking?> GetBookingWithGuideAsync(int bookingId);
    Task<IEnumerable<Message>> GetMessagesAsync(int bookingId, int page, int pageSize);
    Task<int> GetUnreadCountAsync(List<int> bookingIds, string userId);
}