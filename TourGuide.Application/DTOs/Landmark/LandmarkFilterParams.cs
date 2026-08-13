namespace TourGuide.Application.DTOs.Landmark;

public class LandmarkFilterParams
{
    public int? CityId { get; set; }
    public string? Category { get; set; }
    public double? MinRating { get; set; }
    public decimal? MaxPrice { get; set; }
    public string? Search { get; set; }
    public string? SortBy { get; set; } = "rating";
    public string? SortDir { get; set; } = "desc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}