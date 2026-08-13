// ForgetPasswordRequest.cs
namespace TourGuide.Application.DTOs.Auth;

public class ForgetPasswordRequest
{
    public string Email { get; set; } = string.Empty;
}