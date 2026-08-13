namespace TourGuide.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Foreign Keys
    public string TouristId { get; set; } = string.Empty;
    public ApplicationUser Tourist { get; set; } = null!;

    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
}