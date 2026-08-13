namespace TourGuide.Application.DTOs.Booking;

public class CreateBookingRequest
{
    public int? PackageId { get; set; }
    public int GuideProfileId { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfPersons { get; set; }
    public int DurationDays { get; set; }
}