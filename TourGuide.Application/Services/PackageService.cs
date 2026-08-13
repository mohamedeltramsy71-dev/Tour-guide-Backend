using Microsoft.AspNetCore.Http;
using TourGuide.Application.DTOs.Package;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class PackageService : IPackageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudinaryService _cloudinaryService;

    public PackageService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<List<PackageDto>> GetAllPackagesAsync(PackageFilterParams filter)
    {
        var packages = await _unitOfWork.Repository<Package>()
            .FindAsync(p => p.IsActive && !p.IsDeleted);

        var list = packages.ToList();

        if (filter.CityId.HasValue)
            list = list.Where(p => p.CityId == filter.CityId.Value).ToList();

        if (filter.MinPrice.HasValue)
            list = list.Where(p => p.Price >= filter.MinPrice.Value).ToList();

        if (filter.MaxPrice.HasValue)
            list = list.Where(p => p.Price <= filter.MaxPrice.Value).ToList();

        if (filter.DurationDays.HasValue)
            list = list.Where(p => p.DurationDays == filter.DurationDays.Value).ToList();

        if (filter.MinRating.HasValue)
            list = list.Where(p => p.AverageRating >= filter.MinRating.Value).ToList();

        list = filter.SortBy?.ToLower() switch
        {
            "price" => list.OrderBy(p => p.Price).ToList(),
            "rating" => list.OrderByDescending(p => p.AverageRating).ToList(),
            "duration" => list.OrderBy(p => p.DurationDays).ToList(),
            _ => list.OrderByDescending(p => p.CreatedAt).ToList()
        };

        list = list
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToList();

        var result = new List<PackageDto>();
        foreach (var p in list)
            result.Add(await MapToDtoAsync(p));

        return result;
    }

    public async Task<PackageDto> GetPackageByIdAsync(int packageId)
    {
        var package = await _unitOfWork.Repository<Package>()
            .FindOneAsync(p => p.Id == packageId && !p.IsDeleted)
            ?? throw new NotFoundException("Package not found");

        return await MapToDtoAsync(package);
    }

    public async Task<List<PackageDto>> ComparePackagesAsync(List<int> packageIds)
    {
        var result = new List<PackageDto>();
        foreach (var id in packageIds.Take(3))
        {
            var package = await _unitOfWork.Repository<Package>()
                .FindOneAsync(p => p.Id == id && !p.IsDeleted);
            if (package != null)
                result.Add(await MapToDtoAsync(package));
        }
        return result;
    }

    public async Task<PackageDto> CreatePackageAsync(string guideUserId, CreatePackageRequest request)
    {
        var guideProfile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideUserId)
            ?? throw new NotFoundException("Guide profile not found");

        var package = new Package
        {
            Title = request.Title,
            Description = request.Description,
            Price = request.Price,
            DurationDays = request.DurationDays,
            MaxPersons = request.MaxPersons,
            CityId = request.CityId,
            GuideProfileId = guideProfile.Id,
            IsActive = true,
            IsDeleted = false,
            CreatedAt = DateTime.UtcNow
        };

        await _unitOfWork.Repository<Package>().AddAsync(package);
        await _unitOfWork.SaveChangesAsync();

        return await MapToDtoAsync(package);
    }

    public async Task<PackageDto> UpdatePackageAsync(string guideUserId, int packageId, UpdatePackageRequest request)
    {
        var package = await GetOwnPackageAsync(guideUserId, packageId);

        package.Title = request.Title;
        package.Description = request.Description;
        package.Price = request.Price;
        package.DurationDays = request.DurationDays;
        package.MaxPersons = request.MaxPersons;

        _unitOfWork.Repository<Package>().Update(package);
        await _unitOfWork.SaveChangesAsync();

        return await MapToDtoAsync(package);
    }

    public async Task DeletePackageAsync(string guideUserId, int packageId)
    {
        var package = await GetOwnPackageAsync(guideUserId, packageId);

        package.IsDeleted = true;
        _unitOfWork.Repository<Package>().Update(package);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task ToggleActiveAsync(string guideUserId, int packageId)
    {
        var package = await GetOwnPackageAsync(guideUserId, packageId);

        package.IsActive = !package.IsActive;
        _unitOfWork.Repository<Package>().Update(package);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AddLandmarkAsync(string guideUserId, int packageId, AddLandmarkToPackageRequest request)
    {
        var package = await GetOwnPackageAsync(guideUserId, packageId);

        var landmark = await _unitOfWork.Repository<Landmark>()
            .GetByIdAsync(request.LandmarkId)
            ?? throw new NotFoundException("Landmark not found");

        var exists = await _unitOfWork.Repository<PackageLandmark>()
            .ExistsAsync(pl => pl.PackageId == packageId && pl.LandmarkId == request.LandmarkId);

        if (exists)
            throw new ConflictException("Landmark already added to this package");

        await _unitOfWork.Repository<PackageLandmark>().AddAsync(new PackageLandmark
        {
            PackageId = packageId,
            LandmarkId = request.LandmarkId,
            DayNumber = request.DayNumber,
            Order = request.Order
        });

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task RemoveLandmarkAsync(string guideUserId, int packageId, int landmarkId)
    {
        await GetOwnPackageAsync(guideUserId, packageId);

        var pl = await _unitOfWork.Repository<PackageLandmark>()
            .FindOneAsync(pl => pl.PackageId == packageId && pl.LandmarkId == landmarkId)
            ?? throw new NotFoundException("Landmark not found in this package");

        _unitOfWork.Repository<PackageLandmark>().Delete(pl);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<PackageDto> UploadImageAsync(string guideUserId, int packageId, IFormFile image)
    {
        var package = await GetOwnPackageAsync(guideUserId, packageId);

        var url = await _cloudinaryService.UploadImageAsync(image, "packages");

        await _unitOfWork.Repository<PackageImage>().AddAsync(new PackageImage
        {
            PackageId = packageId,
            ImageUrl = url
        });

        await _unitOfWork.SaveChangesAsync();

        return await MapToDtoAsync(package);
    }

    public async Task DeleteImageAsync(string guideUserId, int packageId, int imageId)
    {
        await GetOwnPackageAsync(guideUserId, packageId);

        var image = await _unitOfWork.Repository<PackageImage>()
            .FindOneAsync(i => i.Id == imageId && i.PackageId == packageId)
            ?? throw new NotFoundException("Image not found");

        await _cloudinaryService.DeleteImageAsync(image.ImageUrl);
        _unitOfWork.Repository<PackageImage>().Delete(image);
        await _unitOfWork.SaveChangesAsync();
    }

    // ─── Helpers ───────────────────────────────────────────────

    private async Task<Package> GetOwnPackageAsync(string guideUserId, int packageId)
    {
        var guideProfile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideUserId)
            ?? throw new NotFoundException("Guide profile not found");

        var package = await _unitOfWork.Repository<Package>()
            .FindOneAsync(p => p.Id == packageId && p.GuideProfileId == guideProfile.Id && !p.IsDeleted)
            ?? throw new NotFoundException("Package not found or not owned by this guide");

        return package;
    }

    private async Task<PackageDto> MapToDtoAsync(Package package)
    {
        var city = await _unitOfWork.Repository<City>().GetByIdAsync(package.CityId);

        var guideProfile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.Id == package.GuideProfileId);

        var images = await _unitOfWork.Repository<PackageImage>()
            .FindAsync(i => i.PackageId == package.Id);

        var packageLandmarks = await _unitOfWork.Repository<PackageLandmark>()
            .FindAsync(pl => pl.PackageId == package.Id);

        var landmarkDtos = new List<PackageLandmarkDto>();
        foreach (var pl in packageLandmarks.OrderBy(pl => pl.DayNumber).ThenBy(pl => pl.Order))
        {
            var landmark = await _unitOfWork.Repository<Landmark>().GetByIdAsync(pl.LandmarkId);
            if (landmark != null)
            {
                landmarkDtos.Add(new PackageLandmarkDto
                {
                    LandmarkId = landmark.Id,
                    NameEn = landmark.NameEn,
                    DayNumber = pl.DayNumber,
                    Order = pl.Order
                });
            }
        }

        var guideName = string.Empty;
        var guideUserId = string.Empty;
        if (guideProfile != null)
        {
            guideUserId = guideProfile.UserId;
            var guideUser = await _unitOfWork.Repository<ApplicationUser>()
                .FindOneAsync(u => u.Id == guideProfile.UserId);
            guideName = guideUser?.FullName ?? string.Empty;
        }

        return new PackageDto
        {
            Id = package.Id,
            Title = package.Title,
            Description = package.Description,
            Price = package.Price,
            DurationDays = package.DurationDays,
            MaxPersons = package.MaxPersons,
            IsActive = package.IsActive,
            AverageRating = package.AverageRating,
            CityNameEn = city?.NameEn ?? string.Empty,
            GuideId = guideUserId,
            GuideName = guideName,
            Images = images.Select(i => i.ImageUrl).ToList(),
            Landmarks = landmarkDtos
        };
    }
}