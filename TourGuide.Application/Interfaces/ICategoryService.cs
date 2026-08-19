// TourGuide.Application/Interfaces/ICategoryService.cs

using TourGuide.Application.DTOs.Category;

namespace TourGuide.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto> CreateAsync(CreateCategoryRequest request);
    Task DeleteAsync(int id);
}