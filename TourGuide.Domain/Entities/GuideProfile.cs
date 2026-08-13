namespace TourGuide.Domain.Entities;

public class GuideProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;
    public int ExperienceYears { get; set; }
    public double AverageRating { get; set; } = 0;
    public int TotalReviews { get; set; } = 0;
    public bool IsApproved { get; set; } = false;
    public bool IsAvailable { get; set; } = true;
    public bool IsSuspended { get; set; } = false;
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Languages stored as JSON
    public string LanguagesJson { get; set; } = "[]";

    // Foreign Key
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    // Navigation Properties
    public ICollection<GuideCity> CoveredCities { get; set; } = new List<GuideCity>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}