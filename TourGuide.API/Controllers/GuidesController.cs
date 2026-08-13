using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.Guide;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
public class GuidesController : ControllerBase
{
    private readonly IGuideService _guideService;

    public GuidesController(IGuideService guideService)
    {
        _guideService = guideService;
    }

    [HttpGet("api/guides/me")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _guideService.GetMyProfileAsync(userId);
        return Ok(result);
    }

    [HttpPut("api/guides/me")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateGuideRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _guideService.UpdateMyProfileAsync(userId, request);
        return Ok(result);
    }

    [HttpGet("api/guides/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetGuideById(string id)
    {
        var result = await _guideService.GetGuideByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/guides")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllGuides(
        [FromQuery] int? cityId,
        [FromQuery] string? language,
        [FromQuery] double? minRating)
    {
        var result = await _guideService.GetAllGuidesAsync(cityId, language, minRating);
        return Ok(result);
    }
}

public class RejectGuideRequest
{
    public string Reason { get; set; } = string.Empty;
}