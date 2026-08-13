namespace TourGuide.Application.DTOs.Chat;

public class SendMessageRequest
{
    public string ReceiverId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int BookingId { get; set; }
}