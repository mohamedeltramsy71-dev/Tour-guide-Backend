using TourGuide.Application.DTOs.User;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace TourGuide.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICloudinaryService _cloudinaryService;

    public UserService(
        UserManager<ApplicationUser> userManager,
        ICloudinaryService cloudinaryService)
    {
        _userManager = userManager;
        _cloudinaryService = cloudinaryService;
    }

    // ───── Get My Profile ─────
    public async Task<UserDto> GetMyProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        return MapToDto(user, roles.FirstOrDefault() ?? "");
    }

    // ───── Update My Profile ─────
    public async Task<UserDto> UpdateMyProfileAsync(string userId, UpdateProfileRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        user.FullName = request.FullName;
        user.PhoneNumber = request.Phone;
        user.Bio = request.Bio;

        await _userManager.UpdateAsync(user);

        var roles = await _userManager.GetRolesAsync(user);
        return MapToDto(user, roles.FirstOrDefault() ?? "");
    }

    // ───── Upload Avatar ─────
    public async Task<AvatarResponse> UploadAvatarAsync(string userId, IFormFile file)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        var url = await _cloudinaryService.UploadImageAsync(file, "avatars");

        user.AvatarUrl = url;

        await _userManager.UpdateAsync(user);

        return new AvatarResponse { AvatarUrl = url };
    }

    // ───── Admin: Get All Users ─────
    public async Task<IEnumerable<UserDto>> GetAllUsersAsync(PaginatedUsersRequest request)
    {
        var users = _userManager.Users
            .Where(u => !u.IsDeleted);

        // فلتر Search
        if (!string.IsNullOrEmpty(request.Search))
            users = users.Where(u =>
                u.FullName.Contains(request.Search) ||
                u.Email!.Contains(request.Search));

        // فلتر IsBanned
        if (request.IsBanned.HasValue)
            users = users.Where(u => u.IsBanned == request.IsBanned.Value);

        var pagedUsers = users
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        var result = new List<UserDto>();

        foreach (var user in pagedUsers)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var dto = MapToDto(user, roles.FirstOrDefault() ?? "");

            // فلتر Role بعد ما جبنا الـ roles
            if (!string.IsNullOrEmpty(request.Role) &&
                !string.Equals(dto.Role, request.Role, StringComparison.OrdinalIgnoreCase))
                continue;

            result.Add(dto);
        }

        return result;
    }

    // ───── Admin: Get User By ID ─────
    public async Task<UserDto> GetUserByIdAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        var roles = await _userManager.GetRolesAsync(user);
        return MapToDto(user, roles.FirstOrDefault() ?? "");
    }

    // ───── Admin: Toggle Ban ─────
    public async Task ToggleBanAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        user.IsBanned = !user.IsBanned;
        await _userManager.UpdateAsync(user);
    }

    // ───── Admin: Soft Delete ─────
    public async Task SoftDeleteUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        user.IsDeleted = true;
        await _userManager.UpdateAsync(user);
    }

    // ───── Helper: Map to DTO ─────
    private static UserDto MapToDto(ApplicationUser user, string role) => new()
    {
        Id = user.Id,
        FullName = user.FullName,
        Email = user.Email ?? "",
        Phone = user.PhoneNumber,
        Bio = user.Bio,
        AvatarUrl = user.AvatarUrl,
        Role = role,
        IsBanned = user.IsBanned,
        IsDeleted = user.IsDeleted,
        CreatedAt = user.CreatedAt
    };
}