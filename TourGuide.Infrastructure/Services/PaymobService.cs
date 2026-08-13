using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using TourGuide.Application.Interfaces;

namespace TourGuide.Infrastructure.Services;

public class PaymobService : IPaymobService
{
    private readonly PaymobSettings _settings;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://accept.paymob.com/api";

    public PaymobService(IOptions<PaymobSettings> settings, HttpClient httpClient)
    {
        _settings = settings.Value;
        _httpClient = httpClient;
    }

    // Step 1 — Get Auth Token
    public async Task<string> GetAuthTokenAsync()
    {
        var payload = new { api_key = _settings.ApiKey };
        var response = await PostAsync($"{BaseUrl}/auth/tokens", payload);
        return response.GetProperty("token").GetString()!;
    }

    // Step 2 — Create Order
    public async Task<string> CreateOrderAsync(string authToken, long amountCents, string currency = "EGP")
    {
        var payload = new
        {
            auth_token = authToken,
            delivery_needed = false,
            amount_cents = amountCents,
            currency,
            items = Array.Empty<object>()
        };

        var response = await PostAsync($"{BaseUrl}/ecommerce/orders", payload);
        return response.GetProperty("id").GetInt64().ToString();
    }

    // Step 3 — Get Payment Key
    public async Task<string> GetPaymentKeyAsync(
        string authToken,
        string orderId,
        long amountCents,
        string email,
        string firstName,
        string lastName,
        string currency = "EGP")
    {
        var payload = new
        {
            auth_token = authToken,
            amount_cents = amountCents,
            expiration = 3600,
            order_id = orderId,
            billing_data = new
            {
                email,
                first_name = firstName,
                last_name = lastName,
                phone_number = "NA",
                apartment = "NA",
                floor = "NA",
                street = "NA",
                building = "NA",
                shipping_method = "NA",
                postal_code = "NA",
                city = "NA",
                country = "NA",
                state = "NA"
            },
            currency,
            integration_id = int.Parse(_settings.IntegrationId)
        };

        var response = await PostAsync($"{BaseUrl}/acceptance/payment_keys", payload);
        return response.GetProperty("token").GetString()!;
    }

    // HMAC Validation
    public bool ValidateHmac(Dictionary<string, string> webhookData, string receivedHmac)
    {
        // الحقول المطلوبة من Paymob بالترتيب
        var fields = new[]
        {
            "amount_cents", "created_at", "currency", "error_occured",
            "has_parent_transaction", "id", "integration_id", "is_3d_secure",
            "is_auth", "is_capture", "is_refunded", "is_standalone_payment",
            "is_voided", "order", "owner", "pending",
            "source_data_pan", "source_data_sub_type", "source_data_type", "success"
        };

        var concatenated = string.Concat(fields.Select(f =>
            webhookData.TryGetValue(f, out var val) ? val : ""));

        using var hmac = new HMACSHA512(Encoding.UTF8.GetBytes(_settings.HmacSecret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(concatenated));
        var computed = Convert.ToHexString(hash).ToLower();

        return computed == receivedHmac.ToLower();
    }

    // Helper
    private async Task<JsonElement> PostAsync(string url, object payload)
    {
        var json = JsonSerializer.Serialize(payload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
        var body = await response.Content.ReadAsStringAsync();
        return JsonDocument.Parse(body).RootElement;
    }
}