namespace TourGuide.Domain.Entities;

public class City
{
    public int Id { get; set; }
    public string NameAr { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<Landmark> Landmarks { get; set; } = new List<Landmark>();
    public ICollection<Package> Packages { get; set; } = new List<Package>();
    public ICollection<GuideCity> GuideCities { get; set; } = new List<GuideCity>();
}