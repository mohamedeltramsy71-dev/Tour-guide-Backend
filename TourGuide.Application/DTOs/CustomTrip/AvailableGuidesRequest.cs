namespace TourGuide.Application.DTOs.CustomTrip;

public class AvailableGuidesRequest
{
    public int CityId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}