namespace TourGuide.Application.DTOs.Admin;

public class UserGrowthDto
{
    public List<UserGrowthItem> Items { get; set; } = new();
}

public class UserGrowthItem
{
    public string Period { get; set; } = string.Empty;
    public int NewUsers { get; set; }
}