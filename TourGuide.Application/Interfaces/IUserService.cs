using TourGuide.Application.DTOs.User;
using Microsoft.AspNetCore.Http;

namespace TourGuide.Application.Interfaces;

public interface IUserService
{
    // Tourist / Guide
    Task<UserDto> GetMyProfileAsync(string userId);
    Task<UserDto> UpdateMyProfileAsync(string userId, UpdateProfileRequest request);
    Task<AvatarResponse> UploadAvatarAsync(string userId, IFormFile file);

    // Admin
    Task<IEnumerable<UserDto>> GetAllUsersAsync(PaginatedUsersRequest request);
    Task<UserDto> GetUserByIdAsync(string userId);
    Task ToggleBanAsync(string userId);
    Task SoftDeleteUserAsync(string userId);
}