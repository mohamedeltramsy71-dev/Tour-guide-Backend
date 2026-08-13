namespace TourGuide.Domain.Entities;

public class Package
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int DurationDays { get; set; }
    public int MaxPersons { get; set; }
    public double AverageRating { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int CityId { get; set; }
    public City City { get; set; } = null!;

    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    // Navigation Properties
    public ICollection<PackageLandmark> PackageLandmarks { get; set; } = new List<PackageLandmark>();
    public ICollection<PackageImage> Images { get; set; } = new List<PackageImage>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}