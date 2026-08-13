using TourGuide.Application.DTOs.City;
using Microsoft.AspNetCore.Http;

namespace TourGuide.Application.Interfaces;

public interface ICityService
{
    Task<IEnumerable<CityDto>> GetAllCitiesAsync(int page, int pageSize);
    Task<CityDto> GetCityByIdAsync(int id);
    Task<IEnumerable<CityDto>> GetTrendingCitiesAsync(int topN);
    Task<CityDto> CreateCityAsync(CreateCityRequest request);
    Task<CityDto> UpdateCityAsync(int id, UpdateCityRequest request);
    Task DeleteCityAsync(int id);
}