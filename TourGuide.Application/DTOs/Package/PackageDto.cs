namespace TourGuide.Application.DTOs.Package;

public class PackageDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int MaxPersons { get; set; }
    public bool IsActive { get; set; }
    public double AverageRating { get; set; }
    public string CityNameEn { get; set; } = string.Empty;
    public string GuideId { get; set; } = string.Empty;
    public string GuideName { get; set; } = string.Empty;
    public List<string> Images { get; set; } = [];
    public List<PackageLandmarkDto> Landmarks { get; set; } = [];
}

public class PackageLandmarkDto
{
    public int LandmarkId { get; set; }
    public string NameEn { get; set; } = string.Empty;
    public int DayNumber { get; set; }
    public int Order { get; set; }
}