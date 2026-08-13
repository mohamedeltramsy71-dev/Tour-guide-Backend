using TourGuide.Domain.Enums;

namespace TourGuide.Domain.Entities;

public class Landmark
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Location { get; set; }
    public decimal EntryFee { get; set; } = 0;
    public double AverageRating { get; set; } = 0;
    public LandmarkCategory Category { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int CityId { get; set; }
    public City City { get; set; } = null!;

    // Navigation Properties
    public ICollection<LandmarkImage> Images { get; set; } = new List<LandmarkImage>();
    public ICollection<PackageLandmark> PackageLandmarks { get; set; } = new List<PackageLandmark>();
}