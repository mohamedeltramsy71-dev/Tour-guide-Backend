namespace TourGuide.Domain.Entities;

public class LandmarkImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int LandmarkId { get; set; }
    public Landmark Landmark { get; set; } = null!;
}