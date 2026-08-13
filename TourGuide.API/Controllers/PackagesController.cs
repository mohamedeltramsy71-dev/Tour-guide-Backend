using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TourGuide.Application.DTOs.Package;
using TourGuide.Application.Interfaces;

namespace TourGuide.API.Controllers;

[ApiController]
public class PackagesController : ControllerBase
{
    private readonly IPackageService _packageService;

    public PackagesController(IPackageService packageService)
    {
        _packageService = packageService;
    }

    // ─── Public Endpoints ──────────────────────────────────────

    [HttpGet("api/packages")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllPackages([FromQuery] PackageFilterParams filter)
    {
        var result = await _packageService.GetAllPackagesAsync(filter);
        return Ok(result);
    }

    [HttpGet("api/packages/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackageById(int id)
    {
        var result = await _packageService.GetPackageByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/packages/compare")]
    [AllowAnonymous]
    public async Task<IActionResult> ComparePackages([FromQuery] List<int> ids)
    {
        var result = await _packageService.ComparePackagesAsync(ids);
        return Ok(result);
    }

    // ─── Guide Endpoints ───────────────────────────────────────

    [HttpPost("api/packages")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> CreatePackage([FromBody] CreatePackageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _packageService.CreatePackageAsync(userId, request);
        return Ok(result);
    }

    [HttpPut("api/packages/{id}")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> UpdatePackage(int id, [FromBody] UpdatePackageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _packageService.UpdatePackageAsync(userId, id, request);
        return Ok(result);
    }

    [HttpDelete("api/packages/{id}")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> DeletePackage(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _packageService.DeletePackageAsync(userId, id);
        return Ok(new { message = "Package deleted successfully" });
    }

    [HttpPut("api/packages/{id}/toggle")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> ToggleActive(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _packageService.ToggleActiveAsync(userId, id);
        return Ok(new { message = "Package status toggled" });
    }

    [HttpPost("api/packages/{id}/landmarks")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> AddLandmark(int id, [FromBody] AddLandmarkToPackageRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _packageService.AddLandmarkAsync(userId, id, request);
        return Ok(new { message = "Landmark added successfully" });
    }

    [HttpDelete("api/packages/{id}/landmarks/{landmarkId}")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> RemoveLandmark(int id, int landmarkId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _packageService.RemoveLandmarkAsync(userId, id, landmarkId);
        return Ok(new { message = "Landmark removed successfully" });
    }

    [HttpPost("api/packages/{id}/images")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> UploadImage(int id, IFormFile image)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _packageService.UploadImageAsync(userId, id, image);
        return Ok(result);
    }

    [HttpDelete("api/packages/{id}/images/{imageId}")]
    [Authorize(Roles = "Guide")]
    public async Task<IActionResult> DeleteImage(int id, int imageId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        await _packageService.DeleteImageAsync(userId, id, imageId);
        return Ok(new { message = "Image deleted successfully" });
    }
}