namespace TourGuide.Application.DTOs.Landmark;

public class LandmarkDto
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal EntryFee { get; set; }
    public double AverageRating { get; set; }
    public string Category { get; set; } = string.Empty;
    public int CityId { get; set; }
    public string CityName { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
}