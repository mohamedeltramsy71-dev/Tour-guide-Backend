using TourGuide.Application.DTOs.Landmark;
using Microsoft.AspNetCore.Http;

namespace TourGuide.Application.Interfaces;

public interface ILandmarkService
{
    Task<IEnumerable<LandmarkDto>> GetAllLandmarksAsync(LandmarkFilterParams filter);
    Task<LandmarkDto> GetLandmarkByIdAsync(int id);
    Task<LandmarkDto> CreateLandmarkAsync(CreateLandmarkRequest request);
    Task<LandmarkDto> UpdateLandmarkAsync(int id, UpdateLandmarkRequest request);
    Task DeleteLandmarkAsync(int id);
    Task<string> UploadImageAsync(int landmarkId, IFormFile file);
    Task DeleteImageAsync(int landmarkId, int imageId);
}