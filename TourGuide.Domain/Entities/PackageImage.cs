namespace TourGuide.Domain.Entities;

public class PackageImage
{
    public int Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;
}