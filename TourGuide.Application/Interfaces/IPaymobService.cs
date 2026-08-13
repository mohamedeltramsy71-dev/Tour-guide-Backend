namespace TourGuide.Application.Interfaces;

public interface IPaymobService
{
    Task<string> GetAuthTokenAsync();
    Task<string> CreateOrderAsync(string authToken, long amountCents, string currency = "EGP");
    Task<string> GetPaymentKeyAsync(string authToken, string orderId, long amountCents,
        string email, string firstName, string lastName, string currency = "EGP");
    bool ValidateHmac(Dictionary<string, string> webhookData, string receivedHmac);
}