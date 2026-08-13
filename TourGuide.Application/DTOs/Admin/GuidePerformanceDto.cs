namespace TourGuide.Application.DTOs.Admin;

public class GuidePerformanceDto
{
    public int GuideProfileId { get; set; }
    public string GuideName { get; set; } = string.Empty;
    public double AverageRating { get; set; }
    public int TotalBookings { get; set; }
    public decimal TotalRevenue { get; set; }
}