namespace TourGuide.Application.DTOs.Admin;

public class BookingsReportDto
{
    public List<BookingReportItem> Items { get; set; } = new();
}

public class BookingReportItem
{
    public string Period { get; set; } = string.Empty; // "2025-01-15" or "2025-01"
    public int Count { get; set; }
}