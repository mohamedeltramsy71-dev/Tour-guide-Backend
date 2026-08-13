namespace TourGuide.Application.DTOs.CustomTrip;

public class CreateCustomTripRequest
{
    public List<int> LandmarkIds { get; set; } = new();
    public int GuideProfileId { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfPersons { get; set; }
    public int DurationDays { get; set; }
}