using System.Text.Json.Serialization;

namespace TourGuide.Application.DTOs.Payment;

public class PaymobWebhookDto
{
    [JsonPropertyName("obj")]
    public PaymobTransactionObj? Obj { get; set; }

    [JsonPropertyName("hmac")]
    public string? Hmac { get; set; }
}

public class PaymobTransactionObj
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("amount_cents")]
    public int AmountCents { get; set; }

    [JsonPropertyName("order")]
    public PaymobOrderRef? Order { get; set; }
}

public class PaymobOrderRef
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}