using TourGuide.Application.DTOs.Guide;

namespace TourGuide.Application.Interfaces;

public interface IGuideService
{
    // Guide
    Task<GuideProfileDto> GetMyProfileAsync(string userId);
    Task<GuideProfileDto> UpdateMyProfileAsync(string userId, UpdateGuideRequest request);

    // Public
    Task<GuideProfileDto> GetGuideByIdAsync(string guideId);
    Task<List<GuideListDto>> GetAllGuidesAsync(int? cityId, string? language, double? minRating);

    // Admin
    Task<List<GuideListDto>> GetPendingGuidesAsync();
    Task ApproveGuideAsync(string guideId);
    Task RejectGuideAsync(string guideId, string reason);
    Task ToggleSuspendGuideAsync(string guideId);
}