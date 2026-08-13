namespace TourGuide.Application.DTOs.CustomTrip;

public class CalculatePriceRequest
{
    public List<int> LandmarkIds { get; set; } = new();
    public int DurationDays { get; set; }
    public int NumberOfPersons { get; set; }
    public int GuideProfileId { get; set; }
}