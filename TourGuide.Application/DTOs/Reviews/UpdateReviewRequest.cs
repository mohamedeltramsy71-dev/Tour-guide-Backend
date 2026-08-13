namespace TourGuide.Application.DTOs.Reviews;

public class UpdateReviewRequest
{
    public int Rating { get; set; }
    public string? Comment { get; set; }
}