using TourGuide.Application.DTOs.Reviews;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class ReviewService : IReviewService
{
    private readonly IUnitOfWork _uow;

    public ReviewService(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<ReviewDto> CreateReviewAsync(string touristId, CreateReviewRequest request)
    {
        var booking = await _uow.Repository<Booking>()
            .FindOneAsync(b => b.Id == request.BookingId && b.TouristId == touristId)
            ?? throw new NotFoundException("Booking not found.");

        if (booking.Status != BookingStatus.Completed)
            throw new BusinessRuleException("Can only review a completed booking.");

        var exists = await _uow.Repository<Review>()
            .ExistsAsync(r => r.BookingId == request.BookingId);

        if (exists)
            throw new ConflictException("You already reviewed this booking.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BusinessRuleException("Rating must be between 1 and 5.");

        var review = new Review
        {
            BookingId = request.BookingId,
            TouristId = touristId,
            GuideProfileId = booking.GuideProfileId,
            Rating = request.Rating,
            Comment = request.Comment,
            CreatedAt = DateTime.UtcNow
        };

        await _uow.Repository<Review>().AddAsync(review);
        await _uow.SaveChangesAsync();
        await RecalculateGuideRatingAsync(booking.GuideProfileId);

        return MapToDto(review);
    }

    public async Task<IEnumerable<ReviewDto>> GetGuideReviewsAsync(int guideProfileId, int page, int pageSize)
    {
        var reviews = await _uow.Repository<Review>()
            .FindWithIncludeAsync(r => r.GuideProfileId == guideProfileId, r => r.Tourist);

        var guideProfile = await _uow.Repository<GuideProfile>()
            .FindWithIncludeAsync(g => g.Id == guideProfileId, g => g.User);
        var guide = guideProfile.FirstOrDefault();

        return reviews
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                BookingId = r.BookingId,
                TouristName = r.Tourist?.FullName ?? string.Empty,
                TouristAvatar = r.Tourist?.AvatarUrl,
                GuideName = guide?.User?.FullName ?? string.Empty,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
    }

    public async Task<ReviewDto> UpdateReviewAsync(string touristId, int reviewId, UpdateReviewRequest request)
    {
        var review = await _uow.Repository<Review>()
            .FindOneAsync(r => r.Id == reviewId && r.TouristId == touristId)
            ?? throw new NotFoundException("Review not found.");

        if (request.Rating < 1 || request.Rating > 5)
            throw new BusinessRuleException("Rating must be between 1 and 5.");

        review.Rating = request.Rating;
        review.Comment = request.Comment;
        review.UpdatedAt = DateTime.UtcNow;

        _uow.Repository<Review>().Update(review);
        await _uow.SaveChangesAsync();
        await RecalculateGuideRatingAsync(review.GuideProfileId);

        return MapToDto(review);
    }

    public async Task DeleteReviewAsync(string touristId, int reviewId, bool isAdmin)
    {
        var review = isAdmin
            ? await _uow.Repository<Review>().FindOneAsync(r => r.Id == reviewId)
                ?? throw new NotFoundException("Review not found.")
            : await _uow.Repository<Review>().FindOneAsync(r => r.Id == reviewId && r.TouristId == touristId)
                ?? throw new NotFoundException("Review not found.");

        var guideProfileId = review.GuideProfileId;
        _uow.Repository<Review>().Delete(review);
        await _uow.SaveChangesAsync();
        await RecalculateGuideRatingAsync(guideProfileId);
    }

    public async Task<IEnumerable<ReviewDto>> GetAllReviewsAsync(int page, int pageSize)
    {
        var reviews = await _uow.Repository<Review>()
            .GetAllWithIncludeAsync(r => r.Tourist, r => r.GuideProfile);

        // نجيب الـ GuideProfile Users يدوياً
        var guideProfileIds = reviews.Select(r => r.GuideProfileId).Distinct().ToList();
        var guideProfiles = await _uow.Repository<GuideProfile>()
            .FindWithIncludeAsync(g => guideProfileIds.Contains(g.Id), g => g.User);
        var guideDict = guideProfiles.ToDictionary(g => g.Id);

        return reviews
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(r => new ReviewDto
            {
                Id = r.Id,
                BookingId = r.BookingId,
                TouristName = r.Tourist?.FullName ?? string.Empty,
                TouristAvatar = r.Tourist?.AvatarUrl,
                GuideName = guideDict.TryGetValue(r.GuideProfileId, out var gp) ? gp.User?.FullName ?? string.Empty : string.Empty,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
    }

    private async Task RecalculateGuideRatingAsync(int guideProfileId)
    {
        var reviews = await _uow.Repository<Review>()
            .FindAsync(r => r.GuideProfileId == guideProfileId);

        var guide = await _uow.Repository<GuideProfile>()
            .FindOneAsync(g => g.Id == guideProfileId);

        if (guide is null) return;

        var list = reviews.ToList();
        guide.TotalReviews = list.Count;
        guide.AverageRating = list.Count > 0
            ? Math.Round(list.Average(r => r.Rating), 2)
            : 0;

        _uow.Repository<GuideProfile>().Update(guide);
        await _uow.SaveChangesAsync();
    }

    private static ReviewDto MapToDto(Review r) => new()
    {
        Id = r.Id,
        BookingId = r.BookingId,
        TouristName = r.Tourist?.FullName ?? string.Empty,
        TouristAvatar = r.Tourist?.AvatarUrl,
        Rating = r.Rating,
        Comment = r.Comment,
        CreatedAt = r.CreatedAt
    };
}