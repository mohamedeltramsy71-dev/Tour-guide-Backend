namespace TourGuide.Application.DTOs.Package;

public class CreatePackageRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int MaxPersons { get; set; }
    public int CityId { get; set; }
}