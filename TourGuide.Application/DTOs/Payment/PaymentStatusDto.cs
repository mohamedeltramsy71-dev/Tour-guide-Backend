namespace TourGuide.Application.DTOs.Payment;

public class PaymentStatusDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? PaymobOrderId { get; set; }
    public string? PaymobTransactionId { get; set; }
    public DateTime CreatedAt { get; set; }
}