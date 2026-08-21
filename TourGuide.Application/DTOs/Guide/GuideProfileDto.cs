namespace TourGuide.Application.DTOs.Guide;

public class GuideProfileDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public List<string> Languages { get; set; } = [];
    public int ExperienceYears { get; set; }
    public double AverageRating { get; set; }
    public int TotalReviews { get; set; }
    public bool IsApproved { get; set; }
    public bool IsAvailable { get; set; }
    public List<string> CoveredCities { get; set; } = [];
}