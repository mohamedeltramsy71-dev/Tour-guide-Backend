using Microsoft.AspNetCore.Http;
using TourGuide.Application.DTOs.Package;

namespace TourGuide.Application.Interfaces;

public interface IPackageService
{
    // Public
    Task<List<PackageDto>> GetAllPackagesAsync(PackageFilterParams filter);
    Task<PackageDto> GetPackageByIdAsync(int packageId);
    Task<List<PackageDto>> ComparePackagesAsync(List<int> packageIds);

    // Guide
    Task<PackageDto> CreatePackageAsync(string guideUserId, CreatePackageRequest request);
    Task<PackageDto> UpdatePackageAsync(string guideUserId, int packageId, UpdatePackageRequest request);
    Task DeletePackageAsync(string guideUserId, int packageId);
    Task ToggleActiveAsync(string guideUserId, int packageId);
    Task AddLandmarkAsync(string guideUserId, int packageId, AddLandmarkToPackageRequest request);
    Task RemoveLandmarkAsync(string guideUserId, int packageId, int landmarkId);
    Task<PackageDto> UploadImageAsync(string guideUserId, int packageId, IFormFile image);
    Task DeleteImageAsync(string guideUserId, int packageId, int imageId);
}