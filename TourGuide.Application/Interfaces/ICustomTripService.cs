using TourGuide.Application.DTOs.CustomTrip;
using TourGuide.Application.DTOs.Guide;

namespace TourGuide.Application.Interfaces;

public interface ICustomTripService
{
    Task<CalculatePriceResponse> CalculatePriceAsync(CalculatePriceRequest request);
    Task<List<GuideListDto>> GetAvailableGuidesAsync(AvailableGuidesRequest request);
    Task<int> CreateCustomTripAsync(CreateCustomTripRequest request, string touristId);
}