using TourGuide.Application.DTOs.Booking;

namespace TourGuide.Application.Interfaces;

public interface IBookingService
{
    Task<BookingDto> CreateBookingAsync(CreateBookingRequest request, string touristId);
    Task<IEnumerable<BookingDto>> GetMyBookingsAsync(string touristId, BookingFilterParams filters);
    Task<BookingDto> GetBookingByIdAsync(int id, string userId);
    Task CancelBookingAsync(int id, string touristId);
    Task<IEnumerable<BookingDto>> GetGuideBookingsAsync(int guideProfileId, BookingFilterParams filters);
    Task AcceptBookingAsync(int id, int guideProfileId);
    Task RejectBookingAsync(int id, int guideProfileId, RejectBookingRequest request);
    Task CompleteBookingAsync(int id, string userId);
    Task<IEnumerable<BookingDto>> GetAllBookingsAsync(BookingFilterParams filters); // Admin
    Task<int> GetGuideProfileIdAsync(string userId);
}