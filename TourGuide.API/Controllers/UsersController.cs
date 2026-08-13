using TourGuide.Application.DTOs.User;
using TourGuide.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TourGuide.API.Controllers;

[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("api/users/me")]
    public async Task<IActionResult> GetMyProfile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.GetMyProfileAsync(userId);
        return Ok(result);
    }

    [HttpPut("api/users/me")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.UpdateMyProfileAsync(userId, request);
        return Ok(result);
    }

    [HttpPut("api/users/me/avatar")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
        var result = await _userService.UploadAvatarAsync(userId, file);
        return Ok(result);
    }
}