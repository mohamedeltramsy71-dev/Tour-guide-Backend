using Microsoft.EntityFrameworkCore;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Interfaces;
using TourGuide.Infrastructure.Data;

namespace TourGuide.Infrastructure.Repositories;

public class ChatRepository : IChatRepository
{
    private readonly AppDbContext _context;

    public ChatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetBookingsWithMessagesAsync(string userId)
    {
        return await _context.Bookings
            .Include(b => b.Tourist)
            .Include(b => b.GuideProfile).ThenInclude(g => g.User)
            .Include(b => b.Messages)
            .Where(b => b.TouristId == userId || b.GuideProfile.UserId == userId)
            .Where(b => b.Messages.Any())
            .ToListAsync();
    }

    public async Task<Booking?> GetBookingWithGuideAsync(int bookingId)
    {
        return await _context.Bookings
            .Include(b => b.GuideProfile)
            .FirstOrDefaultAsync(b => b.Id == bookingId);
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(int bookingId, int page, int pageSize)
    {
        return await _context.Messages
            .Include(m => m.Sender)
            .Where(m => m.BookingId == bookingId)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetUnreadCountAsync(List<int> bookingIds, string userId)
    {
        return await _context.Messages
            .Where(m => bookingIds.Contains(m.BookingId)
                     && m.SenderId != userId
                     && !m.IsRead)
            .CountAsync();
    }
}