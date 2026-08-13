using TourGuide.Application.DTOs.Payment;

namespace TourGuide.Application.Interfaces;

public interface IPaymentService
{
    Task<InitiatePaymentResponse> InitiatePaymentAsync(InitiatePaymentRequest request, string touristId);
    Task HandleWebhookAsync(PaymobWebhookDto webhook);
    Task<PaymentStatusDto> GetPaymentStatusAsync(int bookingId, string userId);
}