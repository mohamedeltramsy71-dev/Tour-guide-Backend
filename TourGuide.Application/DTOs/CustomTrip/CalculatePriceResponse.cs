namespace TourGuide.Application.DTOs.CustomTrip;

public class CalculatePriceResponse
{
    public decimal LandmarkEntryFeesTotal { get; set; }
    public decimal GuideFixedFee { get; set; }
    public decimal DurationMultiplier { get; set; }
    public decimal TotalPrice { get; set; }
    public List<LandmarkPriceBreakdown> Breakdown { get; set; } = new();
}

public class LandmarkPriceBreakdown
{
    public int LandmarkId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal EntryFee { get; set; }
}