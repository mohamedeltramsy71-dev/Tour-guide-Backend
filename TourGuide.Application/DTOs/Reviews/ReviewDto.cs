namespace TourGuide.Application.DTOs.Reviews;

public class ReviewDto
{
    public int Id { get; set; }
    public int BookingId { get; set; }
    public string TouristName { get; set; } = string.Empty;
    public string? TouristAvatar { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
}