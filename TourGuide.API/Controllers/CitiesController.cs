using TourGuide.Application.DTOs.City;
using TourGuide.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TourGuide.API.Controllers;

[ApiController]
public class CitiesController : ControllerBase
{
    private readonly ICityService _cityService;

    public CitiesController(ICityService cityService)
    {
        _cityService = cityService;
    }

    // ───── Get All Cities ─────
    [HttpGet("api/cities")]
    public async Task<IActionResult> GetAllCities(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _cityService.GetAllCitiesAsync(page, pageSize);
        return Ok(result);
    }

    // ───── Get City By ID ─────
    [HttpGet("api/cities/{id}")]
    public async Task<IActionResult> GetCityById(int id)
    {
        var result = await _cityService.GetCityByIdAsync(id);
        return Ok(result);
    }

    // ───── Trending Cities ─────
    [HttpGet("api/cities/trending")]
    public async Task<IActionResult> GetTrendingCities([FromQuery] int topN = 5)
    {
        var result = await _cityService.GetTrendingCitiesAsync(topN);
        return Ok(result);
    }

    // ───── Create City (Admin) ─────
    [HttpPost("api/cities")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateCity([FromBody] CreateCityRequest request)
    {
        var result = await _cityService.CreateCityAsync(request);
        return CreatedAtAction(nameof(GetCityById), new { id = result.Id }, result);
    }

    // ───── Update City (Admin) ─────
    [HttpPut("api/cities/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] UpdateCityRequest request)
    {
        var result = await _cityService.UpdateCityAsync(id, request);
        return Ok(result);
    }

    // ───── Delete City (Admin) ─────
    [HttpDelete("api/cities/{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        await _cityService.DeleteCityAsync(id);
        return Ok(new { message = "City deleted successfully" });
    }
}