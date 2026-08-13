using TourGuide.Domain.Entities;

namespace TourGuide.Application.Interfaces;

public interface IJwtService
{
    Task<string> GenerateAccessTokenAsync(ApplicationUser user);
    RefreshToken GenerateRefreshToken(string userId);
}