using TourGuide.Application.DTOs.City;
using TourGuide.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.API.Controllers;

[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly ICloudinaryService _cloudinaryService;

    public CitiesController(ICityService cityService, ICloudinaryService cloudinaryService)
    {
        _cityService = cityService;
        _cloudinaryService = cloudinaryService;
    }

    [HttpGet("api/cities")]
    public async Task<IActionResult> GetAllCities(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _cityService.GetAllCitiesAsync(page, pageSize);
        return Ok(result);
    }

    [HttpGet("api/cities/{id}")]
    public async Task<IActionResult> GetCityById(int id)
    {
        var result = await _cityService.GetCityByIdAsync(id);
        return Ok(result);
    }

    [HttpGet("api/cities/trending")]
    public async Task<IActionResult> GetTrendingCities([FromQuery] int topN = 5)
    {
        var result = await _cityService.GetTrendingCitiesAsync(topN);
        return Ok(result);
    }

    [HttpPost("api/cities")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        var result = await _cityService.CreateCityAsync(request);
        return CreatedAtAction(nameof(GetCityById), new { id = result.Id }, result);
    }

    [HttpPut("api/cities/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityRequest request)
    {
        var result = await _cityService.UpdateCityAsync(id, request);
        return Ok(result);
    }

    [HttpDelete("api/cities/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        await _cityService.DeleteCityAsync(id);
        return Ok(new { message = "City deleted successfully" });
    }

    // ── Upload City Image ──────────────────────────────────
    [HttpPost("api/cities/upload-image")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UploadCityImage(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { message = "No file uploaded" });

        var imageUrl = await _cloudinaryService.UploadImageAsync(file, "cities");
        return Ok(new { imageUrl });
    }
}