using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.Reviews;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewsController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }

    [HttpPost]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _reviewService.CreateReviewAsync(userId, request);
        return Ok(result);
    }

    [HttpGet("guide/{guideProfileId}")]
    public async Task<IActionResult> GetGuideReviews(int guideProfileId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await _reviewService.GetGuideReviewsAsync(guideProfileId, page, pageSize);
        return Ok(result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Tourist")]
    public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _reviewService.UpdateReviewAsync(userId, id, request);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var isAdmin = User.IsInRole("Admin");
        await _reviewService.DeleteReviewAsync(userId, id, isAdmin);
        return NoContent();
    }
}