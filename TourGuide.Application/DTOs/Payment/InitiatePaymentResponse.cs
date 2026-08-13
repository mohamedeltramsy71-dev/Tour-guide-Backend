namespace TourGuide.Application.DTOs.Payment;

public class InitiatePaymentResponse
{
    public string PaymentKey { get; set; } = string.Empty;
    public string PaymobOrderId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}