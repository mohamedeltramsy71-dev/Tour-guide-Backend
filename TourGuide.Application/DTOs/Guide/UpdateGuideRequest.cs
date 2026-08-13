namespace TourGuide.Application.DTOs.Guide;

public class UpdateGuideRequest
{
    public string? Bio { get; set; }
    public List<string> Languages { get; set; } = [];
    public int ExperienceYears { get; set; }
    public List<int> CoveredCityIds { get; set; } = [];
}