using Microsoft.AspNetCore.Http;

namespace TourGuide.Application.Interfaces;

public interface ICloudinaryService
{
    Task<string> UploadImageAsync(IFormFile file, string folder);
    Task DeleteImageAsync(string imageUrl);
}