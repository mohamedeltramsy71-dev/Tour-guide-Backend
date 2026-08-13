namespace TourGuide.Application.DTOs.Admin;

public class TopLandmarkDto
{
    public int LandmarkId { get; set; }
    public string LandmarkName { get; set; } = string.Empty;
    public int InclusionCount { get; set; }
}