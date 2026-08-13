namespace TourGuide.Domain.Entities;

public class GuideCity
{
    public int GuideProfileId { get; set; }
    public GuideProfile GuideProfile { get; set; } = null!;

    public int CityId { get; set; }
    public City City { get; set; } = null!;
}