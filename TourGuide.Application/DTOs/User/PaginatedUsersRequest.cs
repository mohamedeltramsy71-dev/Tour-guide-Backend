namespace TourGuide.Application.DTOs.User;

public class PaginatedUsersRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public string? Role { get; set; }
    public bool? IsBanned { get; set; }
}