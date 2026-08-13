namespace TourGuide.Application.DTOs.Admin;

public class RevenueReportDto
{
    public decimal TotalRevenue { get; set; }
    public List<RevenueReportItem> Items { get; set; } = new();
}

public class RevenueReportItem
{
    public string Period { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}