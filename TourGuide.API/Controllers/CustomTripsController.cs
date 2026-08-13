using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.CustomTrip;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/custom-trips")]
public class CustomTripsController : ControllerBase
{
    private readonly ICustomTripService _customTripService;

    public CustomTripsController(ICustomTripService customTripService)
    {
        _customTripService = customTripService;
    }

    [HttpPost("calculate")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> CalculatePrice([FromBody] CalculatePriceRequest request)
    {
        var result = await _customTripService.CalculatePriceAsync(request);
        return Ok(result);
    }

    [HttpPost("available-guides")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> GetAvailableGuides([FromBody] AvailableGuidesRequest request)
    {
        var guides = await _customTripService.GetAvailableGuidesAsync(request);
        return Ok(guides);
    }

    [HttpPost]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> CreateCustomTrip([FromBody] CreateCustomTripRequest request)
    {
        var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var bookingId = await _customTripService.CreateCustomTripAsync(request, touristId);
        return CreatedAtAction(nameof(CreateCustomTrip), new { id = bookingId },
            new { bookingId, message = "Custom trip created successfully" });
    }
}