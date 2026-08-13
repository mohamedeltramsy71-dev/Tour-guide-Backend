using TourGuide.Domain.Enums;

namespace TourGuide.Domain.Entities;

public class Booking
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public int NumberOfPersons { get; set; }
    public decimal TotalPrice { get; set; }
    public BookingStatus Status { get; set; } = BookingStatus.Pending;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public bool IsCustom { get; set; } = false;
    public string? CustomLandmarksJson { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public string TouristId { get; set; } = string.Empty;
    public ApplicationUser Tourist { get; set; } = null!;

    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public int? PackageId { get; set; }
    public Package? Package { get; set; }

    // Navigation Properties
    public Payment? Payment { get; set; }
    public Review? Review { get; set; }
    public ICollection<Message> Messages { get; set; } = new List<Message>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}