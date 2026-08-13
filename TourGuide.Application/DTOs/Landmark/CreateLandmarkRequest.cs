namespace TourGuide.Application.DTOs.Landmark;

public class CreateLandmarkRequest
{
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal EntryFee { get; set; }
    public string Category { get; set; } = string.Empty;
    public int CityId { get; set; }
}