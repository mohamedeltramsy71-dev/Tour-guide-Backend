namespace TourGuide.Application.DTOs.Admin;

public class TopCityDto
{
    public int CityId { get; set; }
    public string CityName { get; set; } = string.Empty;
    public int BookingCount { get; set; }
}