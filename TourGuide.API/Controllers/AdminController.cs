using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TourGuide.Application.DTOs.User;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;
    private readonly IGuideService _guideService;
    private readonly IUserService _userService;
    private readonly IReviewService _reviewService;

    public AdminController(
        IAdminService adminService,
        IGuideService guideService,
        IUserService userService,
        IReviewService reviewService)
    {
        _adminService = adminService;
        _guideService = guideService;
        _userService = userService;
        _reviewService = reviewService;
    }

    // ─── Dashboard ─────────────────────────────────────────────

    [HttpGet("dashboard")]
    public async Task<IActionResult> GetDashboardSummary()
    {
        var result = await _adminService.GetDashboardSummaryAsync();
        return Ok(result);
    }

    // ─── Reports ───────────────────────────────────────────────

    [HttpGet("reports/bookings")]
    public async Task<IActionResult> GetBookingsReport([FromQuery] string period = "daily")
    {
        var result = await _adminService.GetBookingsReportAsync(period);
        return Ok(result);
    }

    [HttpGet("reports/revenue")]
    public async Task<IActionResult> GetRevenueReport([FromQuery] string period = "monthly")
    {
        var result = await _adminService.GetRevenueReportAsync(period);
        return Ok(result);
    }

    [HttpGet("reports/top-cities")]
    public async Task<IActionResult> GetTopCities([FromQuery] int topN = 5)
    {
        var result = await _adminService.GetTopCitiesAsync(topN);
        return Ok(result);
    }

    [HttpGet("reports/top-landmarks")]
    public async Task<IActionResult> GetTopLandmarks([FromQuery] int topN = 5)
    {
        var result = await _adminService.GetTopLandmarksAsync(topN);
        return Ok(result);
    }

    [HttpGet("reports/guides")]
    public async Task<IActionResult> GetGuidePerformance()
    {
        var result = await _adminService.GetGuidePerformanceReportAsync();
        return Ok(result);
    }

    [HttpGet("reports/users")]
    public async Task<IActionResult> GetUserGrowth([FromQuery] string period = "monthly")
    {
        var result = await _adminService.GetUserGrowthReportAsync(period);
        return Ok(result);
    }

    // ─── Guide Management ──────────────────────────────────────

    [HttpGet("guides/pending")]
    public async Task<IActionResult> GetPendingGuides()
    {
        var result = await _guideService.GetPendingGuidesAsync();
        return Ok(result);
    }

    [HttpPut("guides/{id}/approve")]
    public async Task<IActionResult> ApproveGuide(string id)
    {
        await _guideService.ApproveGuideAsync(id);
        return Ok(new { message = "Guide approved successfully" });
    }

    [HttpPut("guides/{id}/reject")]
    public async Task<IActionResult> RejectGuide(string id, [FromBody] RejectGuideRequest request)
    {
        await _guideService.RejectGuideAsync(id, request.Reason);
        return Ok(new { message = "Guide rejected successfully" });
    }

    [HttpPut("guides/{id}/suspend")]
    public async Task<IActionResult> SuspendGuide(string id)
    {
        await _guideService.ToggleSuspendGuideAsync(id);
        return Ok(new { message = "Guide suspend status toggled" });
    }

    // ─── User Management ───────────────────────────────────────

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers([FromQuery] PaginatedUsersRequest request)
    {
        var result = await _userService.GetAllUsersAsync(request);
        return Ok(result);
    }

    [HttpPut("users/{id}/ban")]
    public async Task<IActionResult> ToggleBan(string id)
    {
        await _userService.ToggleBanAsync(id);
        return Ok(new { message = "User ban status updated successfully" });
    }

    // ─── Review Management ─────────────────────────────────────

    [HttpGet("reviews")]
    public async Task<IActionResult> GetAllReviews(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _reviewService.GetAllReviewsAsync(page, pageSize);
        return Ok(result);
    }

    [HttpDelete("reviews/{id}")]
    public async Task<IActionResult> DeleteReview(int id)
    {
        await _reviewService.DeleteReviewAsync(string.Empty, id, isAdmin: true);
        return NoContent();
    }
}