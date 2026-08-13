using TourGuide.Application.DTOs.Admin;

namespace TourGuide.Application.Interfaces;

public interface IAdminService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<BookingsReportDto> GetBookingsReportAsync(string period); // "daily" or "monthly"
    Task<RevenueReportDto> GetRevenueReportAsync(string period);
    Task<List<TopCityDto>> GetTopCitiesAsync(int topN = 5);
    Task<List<TopLandmarkDto>> GetTopLandmarksAsync(int topN = 5);
    Task<List<GuidePerformanceDto>> GetGuidePerformanceReportAsync();
    Task<UserGrowthDto> GetUserGrowthReportAsync(string period);
}