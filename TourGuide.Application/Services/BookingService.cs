using Microsoft.EntityFrameworkCore;
using TourGuide.Application.DTOs.Booking;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _uow;
    private readonly INotificationService _notificationService;

    public BookingService(IUnitOfWork uow, INotificationService notificationService)
    {
        _uow = uow;
        _notificationService = notificationService;
    }

    public async Task<BookingDto> CreateBookingAsync(CreateBookingRequest request, string touristId)
    {
        decimal totalPrice = 0;
        string? guideUserId = null;

        if (request.PackageId.HasValue)
        {
            var package = await _uow.Repository<Package>().GetByIdAsync(request.PackageId.Value)
                ?? throw new NotFoundException("Package not found");
            totalPrice = package.Price * request.NumberOfPersons;
            request.GuideProfileId = package.GuideProfileId;

            var guideProfile = await _uow.Repository<GuideProfile>().GetByIdAsync(package.GuideProfileId)
                ?? throw new NotFoundException("Guide not found");
            guideUserId = guideProfile.UserId;
        }

        var booking = new Booking
        {
            TouristId = touristId,
            GuideProfileId = request.GuideProfileId,
            PackageId = request.PackageId,
            StartDate = request.StartDate,
            NumberOfPersons = request.NumberOfPersons,
            TotalPrice = totalPrice,
            IsCustom = false,
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Unpaid
        };

        await _uow.Repository<Booking>().AddAsync(booking);
        await _uow.SaveChangesAsync();

        if (guideUserId != null)
        {
            await _notificationService.CreateNotificationAsync(
                guideUserId,
                $"You have a new booking request!",
                NotificationType.NewBooking,
                booking.Id
            );
        }

        return await GetBookingByIdAsync(booking.Id, touristId);
    }

    public async Task<IEnumerable<BookingDto>> GetMyBookingsAsync(string touristId, BookingFilterParams filters)
    {
        var bookings = await _uow.Repository<Booking>().FindWithNestedIncludeAsync(
            b => b.TouristId == touristId &&
                (!filters.FromDate.HasValue || b.StartDate >= filters.FromDate) &&
                (!filters.ToDate.HasValue || b.StartDate <= filters.ToDate),
            q => q.Include(b => b.Tourist)
                  .Include(b => b.GuideProfile).ThenInclude(g => g.User)
                  .Include(b => b.Package)
        );

        return bookings
            .Where(b => string.IsNullOrEmpty(filters.Status) || b.Status.ToString() == filters.Status)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Select(MapToDto);
    }

    public async Task<BookingDto> GetBookingByIdAsync(int id, string userId)
    {
        var bookings = await _uow.Repository<Booking>().FindWithNestedIncludeAsync(
            b => b.Id == id,
            q => q.Include(b => b.Tourist)
                  .Include(b => b.GuideProfile).ThenInclude(g => g.User)
                  .Include(b => b.Package)
        );

        var booking = bookings.FirstOrDefault()
            ?? throw new NotFoundException("Booking not found");

        return MapToDto(booking);
    }

    public async Task CancelBookingAsync(int id, string touristId)
    {
        var booking = await _uow.Repository<Booking>()
            .FindOneAsync(b => b.Id == id && b.TouristId == touristId)
            ?? throw new NotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Pending)
            throw new BusinessRuleException("Only pending bookings can be cancelled");

        booking.Status = BookingStatus.Cancelled;
        _uow.Repository<Booking>().Update(booking);
        await _uow.SaveChangesAsync();
    }

    public async Task<IEnumerable<BookingDto>> GetGuideBookingsAsync(int guideProfileId, BookingFilterParams filters)
    {
        var bookings = await _uow.Repository<Booking>().FindWithNestedIncludeAsync(
            b => b.GuideProfileId == guideProfileId &&
                (!filters.FromDate.HasValue || b.StartDate >= filters.FromDate) &&
                (!filters.ToDate.HasValue || b.StartDate <= filters.ToDate),
            q => q.Include(b => b.Tourist)
                  .Include(b => b.GuideProfile).ThenInclude(g => g.User)
                  .Include(b => b.Package)
        );

        return bookings
            .Where(b => string.IsNullOrEmpty(filters.Status) || b.Status.ToString() == filters.Status)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Select(MapToDto);
    }

    public async Task AcceptBookingAsync(int id, int guideProfileId)
    {
        var booking = await _uow.Repository<Booking>()
            .FindOneAsync(b => b.Id == id && b.GuideProfileId == guideProfileId)
            ?? throw new NotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Pending)
            throw new BusinessRuleException("Only pending bookings can be accepted");

        booking.Status = BookingStatus.Confirmed;
        _uow.Repository<Booking>().Update(booking);
        await _uow.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            booking.TouristId,
            "Your booking has been accepted! You can now proceed with payment.",
            NotificationType.BookingAccepted,
            booking.Id
        );
    }

    public async Task RejectBookingAsync(int id, int guideProfileId, RejectBookingRequest request)
    {
        var booking = await _uow.Repository<Booking>()
            .FindOneAsync(b => b.Id == id && b.GuideProfileId == guideProfileId)
            ?? throw new NotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Pending)
            throw new BusinessRuleException("Only pending bookings can be rejected");

        booking.Status = BookingStatus.Rejected;
        booking.RejectionReason = request.Reason;
        _uow.Repository<Booking>().Update(booking);
        await _uow.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            booking.TouristId,
            $"Your booking has been rejected. Reason: {request.Reason}",
            NotificationType.BookingRejected,
            booking.Id
        );
    }

    public async Task CompleteBookingAsync(int id, string userId)
    {
        var booking = await _uow.Repository<Booking>().FindOneAsync(b => b.Id == id)
            ?? throw new NotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Confirmed)
            throw new BusinessRuleException("Only confirmed bookings can be completed");

        booking.Status = BookingStatus.Completed;
        _uow.Repository<Booking>().Update(booking);
        await _uow.SaveChangesAsync();

        await _notificationService.CreateNotificationAsync(
            booking.TouristId,
            "Your trip has been completed. Please leave a review!",
            NotificationType.TripReminder,
            booking.Id
        );
    }

    public async Task<IEnumerable<BookingDto>> GetAllBookingsAsync(BookingFilterParams filters)
    {
        var bookings = await _uow.Repository<Booking>().FindWithNestedIncludeAsync(
            b =>
                (!filters.FromDate.HasValue || b.StartDate >= filters.FromDate) &&
                (!filters.ToDate.HasValue || b.StartDate <= filters.ToDate),
            q => q.Include(b => b.Tourist)
                  .Include(b => b.GuideProfile).ThenInclude(g => g.User)
                  .Include(b => b.Package)
        );

        return bookings
            .Where(b => string.IsNullOrEmpty(filters.Status) || b.Status.ToString() == filters.Status)
            .OrderByDescending(b => b.CreatedAt)
            .Skip((filters.Page - 1) * filters.PageSize)
            .Take(filters.PageSize)
            .Select(MapToDto);
    }

    public async Task<int> GetGuideProfileIdAsync(string userId)
    {
        var guide = await _uow.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == userId)
            ?? throw new NotFoundException("Guide profile not found");
        return guide.Id;
    }

    private static BookingDto MapToDto(Booking b) => new()
    {
        Id = b.Id,
        StartDate = b.StartDate,
        NumberOfPersons = b.NumberOfPersons,
        TotalPrice = b.TotalPrice,
        Status = b.Status.ToString(),
        PaymentStatus = b.PaymentStatus.ToString(),
        IsCustom = b.IsCustom,
        RejectionReason = b.RejectionReason,
        CreatedAt = b.CreatedAt,
        TouristId = b.TouristId,
        TouristName = b.Tourist?.FullName ?? string.Empty,
        TouristAvatar = b.Tourist?.AvatarUrl,
        GuideProfileId = b.GuideProfileId,
        GuideName = b.GuideProfile?.User?.FullName ?? string.Empty,
        GuideAvatar = b.GuideProfile?.User?.AvatarUrl,
        PackageId = b.PackageId,
        PackageTitle = b.Package?.Title
    };
}