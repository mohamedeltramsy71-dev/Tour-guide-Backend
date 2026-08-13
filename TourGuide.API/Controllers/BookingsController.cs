using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.Booking;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/bookings")]
[Authorize]
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var booking = await _bookingService.CreateBookingAsync(request, touristId);
        return CreatedAtAction(nameof(GetBookingById), new { id = booking.Id }, booking);
    }

    [HttpGet("my")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> GetMyBookings([FromQuery] BookingFilterParams filters)
    {
        var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var bookings = await _bookingService.GetMyBookingsAsync(touristId, filters);
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var booking = await _bookingService.GetBookingByIdAsync(id, userId);
        return Ok(booking);
    }

    [HttpPut("{id}/cancel")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var touristId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _bookingService.CancelBookingAsync(id, touristId);
        return Ok(new { message = "Booking cancelled successfully" });
    }

    [HttpGet("guide")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> GetGuideBookings([FromQuery] BookingFilterParams filters)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var guide = await _bookingService.GetGuideProfileIdAsync(userId);
        var bookings = await _bookingService.GetGuideBookingsAsync(guide, filters);
        return Ok(bookings);
    }

    [HttpPut("{id}/accept")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> AcceptBooking(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var guideProfileId = await _bookingService.GetGuideProfileIdAsync(userId);
        await _bookingService.AcceptBookingAsync(id, guideProfileId);
        return Ok(new { message = "Booking accepted" });
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> RejectBooking(int id, [FromBody] RejectBookingRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var guideProfileId = await _bookingService.GetGuideProfileIdAsync(userId);
        await _bookingService.RejectBookingAsync(id, guideProfileId, request);
        return Ok(new { message = "Booking rejected" });
    }

    [HttpPut("{id}/complete")]
    [Authorize(Roles = "Guide,Admin")]
    public async Task<IActionResult> CompleteBooking(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _bookingService.CompleteBookingAsync(id, userId);
        return Ok(new { message = "Booking marked as completed" });
    }

    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllBookings([FromQuery] BookingFilterParams filters)
    {
        var bookings = await _bookingService.GetAllBookingsAsync(filters);
        return Ok(bookings);
    }
}