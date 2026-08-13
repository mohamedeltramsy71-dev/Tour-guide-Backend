using TourGuide.Domain.Enums;

namespace TourGuide.Domain.Entities;

public class Notification
{
    public int Id { get; set; }
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public string UserId { get; set; } = string.Empty;
    public ApplicationUser User { get; set; } = null!;

    public int? BookingId { get; set; }
    public Booking? Booking { get; set; }
}