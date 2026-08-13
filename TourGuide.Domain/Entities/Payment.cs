using TourGuide.Domain.Enums;

namespace TourGuide.Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public string? PaymobOrderId { get; set; }
    public string? PaymobTransactionId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Key
    public int BookingId { get; set; }
    public Booking Booking { get; set; } = null!;
}