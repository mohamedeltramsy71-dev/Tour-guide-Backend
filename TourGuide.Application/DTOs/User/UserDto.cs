namespace TourGuide.Application.DTOs.User;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Bio { get; set; }
    public string? AvatarUrl { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsBanned { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; }
}