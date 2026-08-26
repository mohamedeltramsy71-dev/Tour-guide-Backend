namespace TourGuide.Application.DTOs.Guide;

public class GuideListDto
{
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public int ExperienceYears { get; set; }
    public List<string> Languages { get; set; } = [];
    public List<string> CoveredCities { get; set; } = [];
    public bool IsAvailable { get; set; }
    public int GuideProfileId { get; set; }
}