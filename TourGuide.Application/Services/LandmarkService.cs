// TourGuide.Application/Services/LandmarkService.cs
// التعديلات: شيلنا كل Enum.TryParse وخلينا Category string مباشرة

using TourGuide.Application.DTOs.Landmark;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace TourGuide.Application.Services;

public class LandmarkService : ILandmarkService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICloudinaryService _cloudinaryService;

    public LandmarkService(IUnitOfWork unitOfWork, ICloudinaryService cloudinaryService)
    {
        _unitOfWork = unitOfWork;
        _cloudinaryService = cloudinaryService;
    }

    public async Task<IEnumerable<LandmarkDto>> GetAllLandmarksAsync(LandmarkFilterParams filter)
    {
        var landmarks = await _unitOfWork.Repository<Landmark>().GetAllAsync();
        var images = await _unitOfWork.Repository<LandmarkImage>().GetAllAsync();
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();

        var query = landmarks.Where(l => !l.IsDeleted);

        if (filter.CityId.HasValue)
            query = query.Where(l => l.CityId == filter.CityId.Value);

        if (!string.IsNullOrEmpty(filter.Category))
            query = query.Where(l => l.Category.ToLower() == filter.Category.ToLower());

        if (filter.MinRating.HasValue)
            query = query.Where(l => l.AverageRating >= filter.MinRating.Value);

        if (filter.MaxPrice.HasValue)
            query = query.Where(l => l.EntryFee <= filter.MaxPrice.Value);

        if (!string.IsNullOrEmpty(filter.Search))
            query = query.Where(l =>
                l.NameAr.Contains(filter.Search) ||
                l.NameEn.Contains(filter.Search));

        query = filter.SortBy?.ToLower() switch
        {
            "price" => filter.SortDir == "asc"
                ? query.OrderBy(l => l.EntryFee)
                : query.OrderByDescending(l => l.EntryFee),
            _ => filter.SortDir == "asc"
                ? query.OrderBy(l => l.AverageRating)
                : query.OrderByDescending(l => l.AverageRating)
        };

        return query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(l => new LandmarkDto
            {
                Id = l.Id,
                NameAr = l.NameAr,
                NameEn = l.NameEn,
                Description = l.Description,
                Location = l.Location,
                EntryFee = l.EntryFee,
                AverageRating = l.AverageRating,
                Category = l.Category,
                CityId = l.CityId,
                CityName = cities.FirstOrDefault(c => c.Id == l.CityId)?.NameEn ?? "",
                Images = images.Where(i => i.LandmarkId == l.Id).Select(i => i.ImageUrl).ToList()
            });
    }

    public async Task<LandmarkDto> GetLandmarkByIdAsync(int id)
    {
        var landmark = await _unitOfWork.Repository<Landmark>().GetByIdAsync(id)
            ?? throw new NotFoundException("Landmark not found");

        if (landmark.IsDeleted)
            throw new NotFoundException("Landmark not found");

        var images = await _unitOfWork.Repository<LandmarkImage>().GetAllAsync();
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();

        return new LandmarkDto
        {
            Id = landmark.Id,
            NameAr = landmark.NameAr,
            NameEn = landmark.NameEn,
            Description = landmark.Description,
            Location = landmark.Location,
            EntryFee = landmark.EntryFee,
            AverageRating = landmark.AverageRating,
            Category = landmark.Category,
            CityId = landmark.CityId,
            CityName = cities.FirstOrDefault(c => c.Id == landmark.CityId)?.NameEn ?? "",
            Images = images.Where(i => i.LandmarkId == landmark.Id).Select(i => i.ImageUrl).ToList()
        };
    }

    public async Task<LandmarkDto> CreateLandmarkAsync(CreateLandmarkRequest request)
    {
        var landmark = new Landmark
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Description = request.Description,
            Location = request.Location,
            EntryFee = request.EntryFee,
            Category = request.Category,
            CityId = request.CityId
        };

        await _unitOfWork.Repository<Landmark>().AddAsync(landmark);
        await _unitOfWork.SaveChangesAsync();

        return new LandmarkDto
        {
            Id = landmark.Id,
            NameAr = landmark.NameAr,
            NameEn = landmark.NameEn,
            Description = landmark.Description,
            Location = landmark.Location,
            EntryFee = landmark.EntryFee,
            AverageRating = landmark.AverageRating,
            Category = landmark.Category,
            CityId = landmark.CityId,
            CityName = "",
            Images = new()
        };
    }

    public async Task<LandmarkDto> UpdateLandmarkAsync(int id, UpdateLandmarkRequest request)
    {
        var landmark = await _unitOfWork.Repository<Landmark>().GetByIdAsync(id)
            ?? throw new NotFoundException("Landmark not found");

        landmark.NameAr = request.NameAr;
        landmark.NameEn = request.NameEn;
        landmark.Description = request.Description;
        landmark.Location = request.Location;
        landmark.EntryFee = request.EntryFee;
        landmark.Category = request.Category;
        landmark.CityId = request.CityId;

        _unitOfWork.Repository<Landmark>().Update(landmark);
        await _unitOfWork.SaveChangesAsync();

        var images = await _unitOfWork.Repository<LandmarkImage>().GetAllAsync();
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();

        return new LandmarkDto
        {
            Id = landmark.Id,
            NameAr = landmark.NameAr,
            NameEn = landmark.NameEn,
            Description = landmark.Description,
            Location = landmark.Location,
            EntryFee = landmark.EntryFee,
            AverageRating = landmark.AverageRating,
            Category = landmark.Category,
            CityId = landmark.CityId,
            CityName = cities.FirstOrDefault(c => c.Id == landmark.CityId)?.NameEn ?? "",
            Images = images.Where(i => i.LandmarkId == landmark.Id).Select(i => i.ImageUrl).ToList()
        };
    }

    public async Task DeleteLandmarkAsync(int id)
    {
        var landmark = await _unitOfWork.Repository<Landmark>().GetByIdAsync(id)
            ?? throw new NotFoundException("Landmark not found");

        landmark.IsDeleted = true;
        _unitOfWork.Repository<Landmark>().Update(landmark);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<string> UploadImageAsync(int landmarkId, IFormFile file)
    {
        var landmark = await _unitOfWork.Repository<Landmark>().GetByIdAsync(landmarkId)
            ?? throw new NotFoundException("Landmark not found");

        var url = await _cloudinaryService.UploadImageAsync(file, "landmarks");

        var image = new LandmarkImage { LandmarkId = landmarkId, ImageUrl = url };
        await _unitOfWork.Repository<LandmarkImage>().AddAsync(image);
        await _unitOfWork.SaveChangesAsync();

        return url;
    }

    public async Task DeleteImageAsync(int landmarkId, int imageId)
    {
        var image = await _unitOfWork.Repository<LandmarkImage>().GetByIdAsync(imageId)
            ?? throw new NotFoundException("Image not found");

        if (image.LandmarkId != landmarkId)
            throw new BusinessRuleException("Image does not belong to this landmark");

        await _cloudinaryService.DeleteImageAsync(image.ImageUrl);
        _unitOfWork.Repository<LandmarkImage>().Delete(image);
        await _unitOfWork.SaveChangesAsync();
    }
}