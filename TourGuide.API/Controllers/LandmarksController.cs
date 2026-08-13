using TourGuide.Application.DTOs.Landmark;
using TourGuide.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.API.Controllers;

[ApiController]
public class LandmarksController : ControllerBase
{
    private readonly ILandmarkService _landmarkService;

    public LandmarksController(ILandmarkService landmarkService)
    {
        _landmarkService = landmarkService;
    }

    // ───── Get All Landmarks ─────
    [HttpGet("api/landmarks")]
    public async Task<IActionResult> GetAllLandmarks([FromQuery] LandmarkFilterParams filter)
    {
        var result = await _landmarkService.GetAllLandmarksAsync(filter);
        return Ok(result);
    }

    // ───── Get Landmark By ID ─────
    [HttpGet("api/landmarks/{id}")]
    public async Task<IActionResult> GetLandmarkById(int id)
    {
        var result = await _landmarkService.GetLandmarkByIdAsync(id);
        return Ok(result);
    }

    // ───── Create Landmark (Admin) ─────
    [HttpPost("api/landmarks")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateLandmark([FromBody] CreateLandmarkRequest request)
    {
        var result = await _landmarkService.CreateLandmarkAsync(request);
        return CreatedAtAction(nameof(GetLandmarkById), new { id = result.Id }, result);
    }

    // ───── Update Landmark (Admin) ─────
    [HttpPut("api/landmarks/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateLandmark(int id, [FromBody] UpdateLandmarkRequest request)
    {
        var result = await _landmarkService.UpdateLandmarkAsync(id, request);
        return Ok(result);
    }

    // ───── Delete Landmark (Admin) ─────
    [HttpDelete("api/landmarks/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteLandmark(int id)
    {
        await _landmarkService.DeleteLandmarkAsync(id);
        return Ok(new { message = "Landmark deleted successfully" });
    }

    // ───── Upload Image (Admin) ─────
    [HttpPost("api/landmarks/{id}/images")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadImage(int id, IFormFile file)
    {
        var url = await _landmarkService.UploadImageAsync(id, file);
        return Ok(new { imageUrl = url });
    }

    // ───── Delete Image (Admin) ─────
    [HttpDelete("api/landmarks/{id}/images/{imgId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteImage(int id, int imgId)
    {
        await _landmarkService.DeleteImageAsync(id, imgId);
        return Ok(new { message = "Image deleted successfully" });
    }
}