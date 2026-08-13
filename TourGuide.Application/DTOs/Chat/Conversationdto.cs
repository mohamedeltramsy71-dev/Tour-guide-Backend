namespace TourGuide.Application.DTOs.Chat;

public class ConversationDto
{
    public int BookingId { get; set; }
    public string OtherUserId { get; set; } = string.Empty;
    public string OtherUserName { get; set; } = string.Empty;
    public string? OtherUserAvatar { get; set; }
    public string LastMessage { get; set; } = string.Empty;
    public DateTime LastMessageAt { get; set; }
    public int UnreadCount { get; set; }
}