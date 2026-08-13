using TourGuide.Application.DTOs.Auth;

namespace TourGuide.Application.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> RegisterAsync(RegisterRequest request, string confirmationBaseUrl);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<LoginResponse> GoogleLoginAsync(GoogleAuthRequest request);
    Task ConfirmEmailAsync(string userId, string token);
    Task ForgetPasswordAsync(ForgetPasswordRequest request, string resetBaseUrl);
    Task ResetPasswordAsync(ResetPasswordRequest request);
    Task<LoginResponse> RefreshTokenAsync(RefreshTokenRequest request);
    Task LogoutAsync(string userId, string refreshToken);
    Task ChangePasswordAsync(string userId, ChangePasswordRequest request);
}