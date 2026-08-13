namespace TourGuide.Domain.Entities;

public class PackageLandmark
{
    public int PackageId { get; set; }
    public Package Package { get; set; } = null!;

    public int LandmarkId { get; set; }
    public Landmark Landmark { get; set; } = null!;

    public int DayNumber { get; set; }
    public int Order { get; set; }
}