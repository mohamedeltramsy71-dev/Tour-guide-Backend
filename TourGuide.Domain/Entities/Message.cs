namespace TourGuide.Domain.Entities;

public class Message
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public string SenderId { get; set; } = string.Empty;
    public ApplicationUser Sender { get; set; } = null!;

    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
}