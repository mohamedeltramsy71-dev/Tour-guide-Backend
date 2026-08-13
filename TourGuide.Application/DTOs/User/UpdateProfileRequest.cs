namespace TourGuide.Application.DTOs.User;

public class UpdateProfileRequest
{
    public string FullName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
}