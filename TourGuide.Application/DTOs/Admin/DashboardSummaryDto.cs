namespace TourGuide.Application.DTOs.Admin;

public class DashboardSummaryDto
{
    public int TotalUsers { get; set; }
    public int TotalGuides { get; set; }
    public int TotalBookingsToday { get; set; }
    public decimal RevenueToday { get; set; }
    public int PendingGuideRequests { get; set; }
}