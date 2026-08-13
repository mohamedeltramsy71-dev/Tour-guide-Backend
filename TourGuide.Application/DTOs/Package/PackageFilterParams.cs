namespace TourGuide.Application.DTOs.Package;

public class PackageFilterParams
{
    public int? CityId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? DurationDays { get; set; }
    public double? MinRating { get; set; }
    public string? SortBy { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}