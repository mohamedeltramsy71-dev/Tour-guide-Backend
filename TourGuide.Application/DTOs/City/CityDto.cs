namespace TourGuide.Application.DTOs.City;

public class CityDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public int LandmarksCount { get; set; }
}