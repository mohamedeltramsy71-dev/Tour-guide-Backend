namespace TourGuide.Application.DTOs.Package;

public class AddLandmarkToPackageRequest
{
    public int LandmarkId { get; set; }
    public int DayNumber { get; set; }
    public int Order { get; set; }
}