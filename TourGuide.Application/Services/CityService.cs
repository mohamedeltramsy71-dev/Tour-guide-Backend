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

        return cities
            .Where(c => !c.IsDeleted)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => MapToDto(c));
    }

    // ───── Get City By ID ─────
    public async Task<CityDto> GetCityByIdAsync(int id)
    {
        var city = await _unitOfWork.Repository<City>().GetByIdAsync(id)
            ?? throw new NotFoundException("City not found");

        if (city.IsDeleted)
            throw new NotFoundException("City not found");

        return MapToDto(city);
    }

    // ───── Trending Cities ─────
    public async Task<IEnumerable<CityDto>> GetTrendingCitiesAsync(int topN)
    {
        var cities = await _unitOfWork.Repository<City>().GetAllAsync();

        return cities
            .Where(c => !c.IsDeleted)
            .Take(topN)
            .Select(c => MapToDto(c));
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

        return MapToDto(city);
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

        return MapToDto(city);
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

    // ───── Helper ─────
    private static CityDto MapToDto(City city) => new()
    {
        Id = city.Id,
        NameAr = city.NameAr,
        NameEn = city.NameEn,
        Description = city.Description,
        ImageUrl = city.ImageUrl,
        LandmarksCount = city.Landmarks?.Count(l => !l.IsDeleted) ?? 0
    };
}