using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TourGuide.Application.DTOs.CustomTrip;
using TourGuide.Application.DTOs.Guide;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class CustomTripService : ICustomTripService
{
    private readonly IUnitOfWork _uow;

    public CustomTripService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<CalculatePriceResponse> CalculatePriceAsync(CalculatePriceRequest request)
    {
        var landmarks = await _uow.Repository<Landmark>()
            .FindAsync(l => request.LandmarkIds.Contains(l.Id));

        var guide = await _uow.Repository<GuideProfile>()
            .GetByIdAsync(request.GuideProfileId)
            ?? throw new NotFoundException("Guide not found");

        var entryFeesTotal = landmarks.Sum(l => l.EntryFee) * request.NumberOfPersons;
        var durationMultiplier = 1 + (request.DurationDays - 1) * 0.2m;
        var total = entryFeesTotal * durationMultiplier;

        return new CalculatePriceResponse
        {
            LandmarkEntryFeesTotal = entryFeesTotal,
            GuideFixedFee = 0,
            DurationMultiplier = durationMultiplier,
            TotalPrice = total,
            Breakdown = landmarks.Select(l => new LandmarkPriceBreakdown
            {
                LandmarkId = l.Id,
                Name = l.NameEn,
                EntryFee = l.EntryFee
            }).ToList()
        };
    }

    public async Task<List<GuideListDto>> GetAvailableGuidesAsync(AvailableGuidesRequest request)
    {
        var allGuides = await _uow.Repository<GuideProfile>()
            .FindWithNestedIncludeAsync(
                g => g.IsApproved && g.IsAvailable && !g.IsSuspended,
                q => q.Include(g => g.User)
                      .Include(g => g.CoveredCities)
                          .ThenInclude(gc => gc.City)
            );

        var guides = allGuides
            .Where(g => g.CoveredCities.Any(gc => gc.CityId == request.CityId))
            .ToList();

        var busyBookings = await _uow.Repository<Booking>()
            .FindAsync(b =>
                (b.Status == BookingStatus.Pending || b.Status == BookingStatus.Confirmed) &&
                b.StartDate < request.EndDate &&
                b.StartDate > request.StartDate.AddDays(-1));

        var busyGuideIds = busyBookings.Select(b => b.GuideProfileId).ToHashSet();

        return guides
            .Where(g => !busyGuideIds.Contains(g.Id))
            .Select(g => new GuideListDto
            {
                GuideProfileId = g.Id,
                UserId = g.UserId,
                FullName = g.User.FullName,
                AvatarUrl = g.User.AvatarUrl,
                AverageRating = g.AverageRating,
                TotalReviews = g.TotalReviews,
                ExperienceYears = g.ExperienceYears,
                IsAvailable = g.IsAvailable,
                Languages = JsonSerializer.Deserialize<List<string>>(g.LanguagesJson) ?? new(),
                CoveredCities = g.CoveredCities.Select(gc => gc.City.NameEn).ToList()
            })
            .ToList();
    }

    public async Task<int> CreateCustomTripAsync(CreateCustomTripRequest request, string touristId)
    {
        var priceResult = await CalculatePriceAsync(new CalculatePriceRequest
        {
            LandmarkIds = request.LandmarkIds,
            GuideProfileId = request.GuideProfileId,
            DurationDays = request.DurationDays,
            NumberOfPersons = request.NumberOfPersons
        });

        var booking = new Booking
        {
            TouristId = touristId,
            GuideProfileId = request.GuideProfileId,
            StartDate = request.StartDate,
            NumberOfPersons = request.NumberOfPersons,
            TotalPrice = priceResult.TotalPrice,
            IsCustom = true,
            CustomLandmarksJson = JsonSerializer.Serialize(request.LandmarkIds),
            Status = BookingStatus.Pending,
            PaymentStatus = PaymentStatus.Unpaid
        };

        await _uow.Repository<Booking>().AddAsync(booking);
        await _uow.SaveChangesAsync();

        return booking.Id;
    }
}