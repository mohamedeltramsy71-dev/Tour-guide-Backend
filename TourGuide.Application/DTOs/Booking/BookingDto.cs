namespace TourGuide.Application.DTOs.Booking;

public class BookingDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfPersons { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public bool IsCustom { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; }

    // Tourist info
    public string TouristId { get; set; } = string.Empty;
    public string TouristName { get; set; } = string.Empty;
    public string? TouristAvatar { get; set; }

    // Guide info
    public int GuideProfileId { get; set; }
    public string GuideUserId { get; set; } = string.Empty;
    public string GuideName { get; set; } = string.Empty;
    public string? GuideAvatar { get; set; }

    // Package info (null if custom)
    public int? PackageId { get; set; }
    public string? PackageTitle { get; set; }
}