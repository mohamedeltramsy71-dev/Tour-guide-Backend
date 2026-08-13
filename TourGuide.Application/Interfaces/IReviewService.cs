using TourGuide.Application.DTOs.Reviews;

namespace TourGuide.Application.Interfaces;

public interface IReviewService
{
    Task<ReviewDto> CreateReviewAsync(string touristId, CreateReviewRequest request);
    Task<IEnumerable<ReviewDto>> GetGuideReviewsAsync(int guideProfileId, int page, int pageSize);
    Task<ReviewDto> UpdateReviewAsync(string touristId, int reviewId, UpdateReviewRequest request);
    Task DeleteReviewAsync(string touristId, int reviewId, bool isAdmin);
    Task<IEnumerable<ReviewDto>> GetAllReviewsAsync(int page, int pageSize);
}