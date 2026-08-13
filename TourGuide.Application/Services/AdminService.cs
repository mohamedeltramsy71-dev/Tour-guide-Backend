using Microsoft.AspNetCore.Identity;
using TourGuide.Application.DTOs.Admin;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Enums;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _uow;
    private readonly UserManager<ApplicationUser> _userManager;

    public AdminService(IUnitOfWork uow, UserManager<ApplicationUser> userManager)
    {
        _uow = uow;
        _userManager = userManager;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var today = DateTime.UtcNow.Date;

        var totalUsers = _userManager.Users.Count(u => !u.IsDeleted);

        var guides = await _userManager.GetUsersInRoleAsync("Guide");
        var totalGuides = guides.Count(u => !u.IsDeleted);

        var bookings = await _uow.Repository<Booking>().GetAllAsync();
        var bookingList = bookings.ToList();

        var totalBookingsToday = bookingList
            .Count(b => b.CreatedAt.Date == today);

        var revenueToday = bookingList
            .Where(b => b.CreatedAt.Date == today && b.PaymentStatus == PaymentStatus.Paid)
            .Sum(b => b.TotalPrice);

        var guides2 = await _uow.Repository<GuideProfile>().GetAllAsync();
        var pendingGuideRequests = guides2.Count(g => !g.IsApproved && !g.IsSuspended);

        return new DashboardSummaryDto
        {
            TotalUsers = totalUsers,
            TotalGuides = totalGuides,
            TotalBookingsToday = totalBookingsToday,
            RevenueToday = revenueToday,
            PendingGuideRequests = pendingGuideRequests
        };
    }

    public async Task<BookingsReportDto> GetBookingsReportAsync(string period)
    {
        var bookings = await _uow.Repository<Booking>().GetAllAsync();
        var bookingList = bookings.ToList();

        List<BookingReportItem> items;

        if (period == "monthly")
        {
            items = bookingList
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new BookingReportItem
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Count = g.Count()
                }).ToList();
        }
        else // daily
        {
            items = bookingList
                .GroupBy(b => b.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new BookingReportItem
                {
                    Period = g.Key.ToString("yyyy-MM-dd"),
                    Count = g.Count()
                }).ToList();
        }

        return new BookingsReportDto { Items = items };
    }

    public async Task<RevenueReportDto> GetRevenueReportAsync(string period)
    {
        var bookings = await _uow.Repository<Booking>().GetAllAsync();
        var paidBookings = bookings
            .Where(b => b.PaymentStatus == PaymentStatus.Paid)
            .ToList();

        List<RevenueReportItem> items;

        if (period == "monthly")
        {
            items = paidBookings
                .GroupBy(b => new { b.CreatedAt.Year, b.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new RevenueReportItem
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Amount = g.Sum(b => b.TotalPrice)
                }).ToList();
        }
        else // weekly
        {
            items = paidBookings
                .GroupBy(b => System.Globalization.CultureInfo.CurrentCulture
                    .Calendar.GetWeekOfYear(b.CreatedAt,
                        System.Globalization.CalendarWeekRule.FirstDay,
                        DayOfWeek.Monday))
                .OrderBy(g => g.Key)
                .Select(g => new RevenueReportItem
                {
                    Period = $"Week {g.Key}",
                    Amount = g.Sum(b => b.TotalPrice)
                }).ToList();
        }

        return new RevenueReportDto
        {
            TotalRevenue = paidBookings.Sum(b => b.TotalPrice),
            Items = items
        };
    }

    public async Task<List<TopCityDto>> GetTopCitiesAsync(int topN = 5)
    {
        var bookings = await _uow.Repository<Booking>().GetAllAsync();
        var packages = await _uow.Repository<Package>().GetAllAsync();

        var result = bookings
            .Where(b => b.PackageId.HasValue)
            .Join(packages,
                b => b.PackageId,
                p => p.Id,
                (b, p) => new { p.CityId, p.City })
            .GroupBy(x => new { x.CityId, x.City.NameEn })
            .OrderByDescending(g => g.Count())
            .Take(topN)
            .Select(g => new TopCityDto
            {
                CityId = g.Key.CityId,
                CityName = g.Key.NameEn,
                BookingCount = g.Count()
            }).ToList();

        return result;
    }

    public async Task<List<TopLandmarkDto>> GetTopLandmarksAsync(int topN = 5)
    {
        var packageLandmarks = await _uow.Repository<PackageLandmark>().GetAllAsync();
        var landmarks = await _uow.Repository<Landmark>().GetAllAsync();

        var result = packageLandmarks
            .Join(landmarks,
                pl => pl.LandmarkId,
                l => l.Id,
                (pl, l) => new { l.Id, l.NameEn })
            .GroupBy(x => new { x.Id, x.NameEn })
            .OrderByDescending(g => g.Count())
            .Take(topN)
            .Select(g => new TopLandmarkDto
            {
                LandmarkId = g.Key.Id,
                LandmarkName = g.Key.NameEn,
                InclusionCount = g.Count()
            }).ToList();

        return result;
    }

    public async Task<List<GuidePerformanceDto>> GetGuidePerformanceReportAsync()
    {
        var guides = await _uow.Repository<GuideProfile>().GetAllAsync();
        var bookings = await _uow.Repository<Booking>().GetAllAsync();
        var bookingList = bookings.ToList();

        var result = guides.Select(g => new GuidePerformanceDto
        {
            GuideProfileId = g.Id,
            GuideName = g.User?.FullName ?? "Unknown",
            AverageRating = g.AverageRating,
            TotalBookings = bookingList.Count(b => b.GuideProfileId == g.Id),
            TotalRevenue = bookingList
                .Where(b => b.GuideProfileId == g.Id && b.PaymentStatus == PaymentStatus.Paid)
                .Sum(b => b.TotalPrice)
        }).ToList();

        return result;
    }

    public async Task<UserGrowthDto> GetUserGrowthReportAsync(string period)
    {
        var users = _userManager.Users
            .Where(u => !u.IsDeleted)
            .ToList();

        List<UserGrowthItem> items;

        if (period == "monthly")
        {
            items = users
                .GroupBy(u => new { u.CreatedAt.Year, u.CreatedAt.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .Select(g => new UserGrowthItem
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    NewUsers = g.Count()
                }).ToList();
        }
        else // daily
        {
            items = users
                .GroupBy(u => u.CreatedAt.Date)
                .OrderBy(g => g.Key)
                .Select(g => new UserGrowthItem
                {
                    Period = g.Key.ToString("yyyy-MM-dd"),
                    NewUsers = g.Count()
                }).ToList();
        }

        return new UserGrowthDto { Items = items };
    }
}