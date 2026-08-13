using TourGuide.Application.DTOs.Payment;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class PaymentService : IPaymentService
{
    private readonly IUnitOfWork _uow;
    private readonly IPaymobService _paymobService;

    public PaymentService(IUnitOfWork uow, IPaymobService paymobService)
    {
        _uow = uow;
        _paymobService = paymobService;
    }

    public async Task<InitiatePaymentResponse> InitiatePaymentAsync(
        InitiatePaymentRequest request, string touristId)
    {
        var booking = await _uow.Repository<Booking>()
            .FindOneAsync(b => b.Id == request.BookingId && b.TouristId == touristId)
            ?? throw new NotFoundException("Booking not found");

        if (booking.Status != BookingStatus.Confirmed)
            throw new BusinessRuleException("Booking must be confirmed before payment");

        if (booking.PaymentStatus == PaymentStatus.Paid)
            throw new BusinessRuleException("Booking is already paid");
        // Paymob flow
        var authToken = await _paymobService.GetAuthTokenAsync();
        var amountCents = (long)(booking.TotalPrice * 100);

        // جيب بيانات الـ tourist
        var tourist = await _uow.Repository<ApplicationUser>()
            .FindOneAsync(u => u.Id == touristId)
            ?? throw new NotFoundException("Tourist not found");

        var nameParts = tourist.FullName.Split(' ');
        var firstName = nameParts[0];
        var lastName = nameParts.Length > 1 ? nameParts[^1] : "NA";

        var paymobOrderId = await _paymobService.CreateOrderAsync(authToken, amountCents);
        var paymentKey = await _paymobService.GetPaymentKeyAsync(
            authToken, paymobOrderId, amountCents,
            tourist.Email!, firstName, lastName);

        // Save Payment record
        var existingPayment = await _uow.Repository<Payment>()
            .FindOneAsync(p => p.BookingId == booking.Id);

        if (existingPayment == null)
        {
            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = booking.TotalPrice,
                PaymobOrderId = paymobOrderId.ToString(),
                Status = PaymentStatus.Unpaid
            };
            await _uow.Repository<Payment>().AddAsync(payment);
        }
        else
        {
            existingPayment.PaymobOrderId = paymobOrderId.ToString();
            _uow.Repository<Payment>().Update(existingPayment);
        }

        await _uow.SaveChangesAsync();

        return new InitiatePaymentResponse
        {
            PaymentKey = paymentKey,
            PaymobOrderId = paymobOrderId.ToString(),
            Amount = booking.TotalPrice
        };
    }

    public async Task HandleWebhookAsync(PaymobWebhookDto webhook)
    {
        if (webhook.Obj == null) return;

        var paymobOrderId = webhook.Obj.Order?.Id.ToString();
        if (string.IsNullOrEmpty(paymobOrderId)) return;

        var payment = await _uow.Repository<Payment>()
            .FindOneAsync(p => p.PaymobOrderId == paymobOrderId);
        if (payment == null) return;

        payment.PaymobTransactionId = webhook.Obj.Id.ToString();
        payment.Status = webhook.Obj.Success ? PaymentStatus.Paid : PaymentStatus.Failed;

        var booking = await _uow.Repository<Booking>().GetByIdAsync(payment.BookingId);
        if (booking != null)
        {
            booking.PaymentStatus = payment.Status;
        }

        _uow.Repository<Payment>().Update(payment);
        if (booking != null) _uow.Repository<Booking>().Update(booking);
        await _uow.SaveChangesAsync();
    }

    public async Task<PaymentStatusDto> GetPaymentStatusAsync(int bookingId, string userId)
    {
        var payment = await _uow.Repository<Payment>()
            .FindOneAsync(p => p.BookingId == bookingId)
            ?? throw new NotFoundException("Payment not found");

        return new PaymentStatusDto
        {
            Id = payment.Id,
            BookingId = payment.BookingId,
            Amount = payment.Amount,
            Status = payment.Status.ToString(),
            PaymobOrderId = payment.PaymobOrderId,
            PaymobTransactionId = payment.PaymobTransactionId,
            CreatedAt = payment.CreatedAt
        };
    }
}