// TourGuide.Application/Services/CategoryService.cs

using TourGuide.Application.DTOs.Category;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly IUnitOfWork _uow;

    public CategoryService(IUnitOfWork uow) => _uow = uow;

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var cats = await _uow.Repository<Category>().GetAllAsync();
        return cats.OrderBy(c => c.Name).Select(c => new CategoryDto { Id = c.Id, Name = c.Name });
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new BusinessRuleException("Category name is required.");

        var exists = await _uow.Repository<Category>()
            .ExistsAsync(c => c.Name.ToLower() == request.Name.ToLower());
        if (exists)
            throw new ConflictException("Category already exists.");

        var category = new Category { Name = request.Name.Trim() };
        await _uow.Repository<Category>().AddAsync(category);
        await _uow.SaveChangesAsync();

        return new CategoryDto { Id = category.Id, Name = category.Name };
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _uow.Repository<Category>().GetByIdAsync(id)
            ?? throw new NotFoundException("Category not found.");

        _uow.Repository<Category>().Delete(category);
        await _uow.SaveChangesAsync();
    }
}