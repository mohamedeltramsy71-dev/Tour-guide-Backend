using TourGuide.Application.DTOs.City;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class CityService : ICityService
{
    private readonly IUnitOfWork _unitOfWork;

    public CityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // ───── Get All Cities ─────
    public async Task<IEnumerable<CityDto>> GetAllCitiesAsync(int page, int pageSize)
    {
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();
        var landmarks = await _unitOfWork.Repository<Landmark>().GetAllAsync();

        return cities
            .Where(c => !c.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CityDto
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                LandmarksCount = landmarks.Count(l => l.CityId == c.Id && !l.IsDeleted)
            });
    }

    // ───── Get City By ID ─────
    public async Task<CityDto> GetCityByIdAsync(int id)
    {
        var city = await _unitOfWork.Repository<City>().GetByIdAsync(id)
            ?? throw new NotFoundException("City not found");

        if (city.IsDeleted)
            throw new NotFoundException("City not found");

        var landmarks = await _unitOfWork.Repository<Landmark>().GetAllAsync();

        return new CityDto
        {
            Id = city.Id,
            NameAr = city.NameAr,
            NameEn = city.NameEn,
            Description = city.Description,
            ImageUrl = city.ImageUrl,
            LandmarksCount = landmarks.Count(l => l.CityId == city.Id && !l.IsDeleted)
        };
    }

    // ───── Trending Cities ─────
    public async Task<IEnumerable<CityDto>> GetTrendingCitiesAsync(int topN)
    {
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();
        var landmarks = await _unitOfWork.Repository<Landmark>().GetAllAsync();

        return cities
            .Where(c => !c.IsDeleted)
            .Take(topN)
            .Select(c => new CityDto
            {
                Id = c.Id,
                NameAr = c.NameAr,
                NameEn = c.NameEn,
                Description = c.Description,
                ImageUrl = c.ImageUrl,
                LandmarksCount = landmarks.Count(l => l.CityId == c.Id && !l.IsDeleted)
            });
    }

    // ───── Create City ─────
    public async Task<CityDto> CreateCityAsync(CreateCityRequest request)
    {
        var city = new City
        {
            NameAr = request.NameAr,
            NameEn = request.NameEn,
            Description = request.Description,
            ImageUrl = request.ImageUrl
        };

        await _unitOfWork.Repository<City>().AddAsync(city);
        await _unitOfWork.SaveChangesAsync();

        return new CityDto
        {
            Id = city.Id,
            NameAr = city.NameAr,
            NameEn = city.NameEn,
            Description = city.Description,
            ImageUrl = city.ImageUrl,
            LandmarksCount = 0
        };
    }

    // ───── Update City ─────
    public async Task<CityDto> UpdateCityAsync(int id, UpdateCityRequest request)
    {
        var city = await _unitOfWork.Repository<City>().GetByIdAsync(id)
            ?? throw new NotFoundException("City not found");

        city.NameAr = request.NameAr;
        city.NameEn = request.NameEn;
        city.Description = request.Description;
        city.ImageUrl = request.ImageUrl;

        _unitOfWork.Repository<City>().Update(city);
        await _unitOfWork.SaveChangesAsync();

        var landmarks = await _unitOfWork.Repository<Landmark>().GetAllAsync();

        return new CityDto
        {
            Id = city.Id,
            NameAr = city.NameAr,
            NameEn = city.NameEn,
            Description = city.Description,
            ImageUrl = city.ImageUrl,
            LandmarksCount = landmarks.Count(l => l.CityId == city.Id && !l.IsDeleted)
        };
    }

    // ───── Delete City ─────
    public async Task DeleteCityAsync(int id)
    {
        var city = await _unitOfWork.Repository<City>().GetByIdAsync(id)
            ?? throw new NotFoundException("City not found");

        city.IsDeleted = true;
        _unitOfWork.Repository<City>().Update(city);
        await _unitOfWork.SaveChangesAsync();
    }
}