using Microsoft.AspNetCore.Identity;
using System.Text.Json;
using TourGuide.Application.DTOs.Guide;
using TourGuide.Application.Interfaces;
using TourGuide.Domain.Entities;
using TourGuide.Domain.Exceptions;
using TourGuide.Domain.Interfaces;

namespace TourGuide.Application.Services;

public class GuideService : IGuideService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmailService _emailService;

    public GuideService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, IEmailService emailService)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _emailService = emailService;
    }

    public async Task<GuideProfileDto> GetMyProfileAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == userId)
            ?? throw new NotFoundException("Guide profile not found");

        var cities = await _unitOfWork.Repository<GuideCity>()
            .FindAsync(gc => gc.GuideProfileId == profile.Id);

        var cityList = new List<string>();
        foreach (var gc in cities)
        {
            var city = await _unitOfWork.Repository<City>()
                .GetByIdAsync(gc.CityId);
            if (city != null) cityList.Add(city.NameEn);
        }

        return MapToProfileDto(user, profile, cityList);
    }

    public async Task<GuideProfileDto> UpdateMyProfileAsync(string userId, UpdateGuideRequest request)
    {
        var user = await _userManager.FindByIdAsync(userId)
            ?? throw new NotFoundException("User not found");

        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == userId)
            ?? throw new NotFoundException("Guide profile not found");

        profile.Bio = request.Bio ?? string.Empty;
        profile.LanguagesJson = JsonSerializer.Serialize(request.Languages);
        profile.ExperienceYears = request.ExperienceYears;

        // Update covered cities
        var existingCities = await _unitOfWork.Repository<GuideCity>()
            .FindAsync(gc => gc.GuideProfileId == profile.Id);

        foreach (var city in existingCities)
            _unitOfWork.Repository<GuideCity>().Delete(city);

        foreach (var cityId in request.CoveredCityIds)
        {
            await _unitOfWork.Repository<GuideCity>().AddAsync(new GuideCity
            {
                GuideProfileId = profile.Id,
                CityId = cityId
            });
        }

        _unitOfWork.Repository<GuideProfile>().Update(profile);
        await _unitOfWork.SaveChangesAsync();

        return await GetMyProfileAsync(userId);
    }

    public async Task<GuideProfileDto> GetGuideByIdAsync(string guideId)
    {
        var user = await _userManager.FindByIdAsync(guideId)
            ?? throw new NotFoundException("Guide not found");

        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideId)
            ?? throw new NotFoundException("Guide profile not found");

        var cities = await _unitOfWork.Repository<GuideCity>()
            .FindAsync(gc => gc.GuideProfileId == profile.Id);

        var cityList = new List<string>();
        foreach (var gc in cities)
        {
            var city = await _unitOfWork.Repository<City>()
                .GetByIdAsync(gc.CityId);
            if (city != null) cityList.Add(city.NameEn);
        }

        return MapToProfileDto(user, profile, cityList);
    }

    public async Task<List<GuideListDto>> GetAllGuidesAsync(int? cityId, string? language, double? minRating)
    {
        var profiles = await _unitOfWork.Repository<GuideProfile>()
            .FindAsync(g => g.IsApproved && !g.IsSuspended);

        var result = new List<GuideListDto>();

        foreach (var profile in profiles)
        {
            var user = await _userManager.FindByIdAsync(profile.UserId);
            if (user == null || user.IsDeleted || user.IsBanned) continue;

            var guideCities = await _unitOfWork.Repository<GuideCity>()
                .FindAsync(gc => gc.GuideProfileId == profile.Id);

            var guideCityList = guideCities.ToList();

            if (cityId.HasValue && !guideCityList.Any(gc => gc.CityId == cityId.Value))
                continue;

            if (minRating.HasValue && profile.AverageRating < minRating.Value)
                continue;

            var langs = JsonSerializer.Deserialize<List<string>>(profile.LanguagesJson ?? "[]") ?? [];

            if (!string.IsNullOrWhiteSpace(language) &&
                !langs.Any(l => l.Contains(language, StringComparison.OrdinalIgnoreCase)))
                continue;

            var cityNames = new List<string>();
            foreach (var gc in guideCityList)
            {
                var city = await _unitOfWork.Repository<City>().GetByIdAsync(gc.CityId);
                if (city != null) cityNames.Add(city.NameEn);
            }

            result.Add(MapToListDto(user, profile, langs, cityNames));
        }

        return result;
    }

    public async Task<List<GuideListDto>> GetPendingGuidesAsync()
    {
        var profiles = await _unitOfWork.Repository<GuideProfile>()
            .FindAsync(g => !g.IsApproved);

        var result = new List<GuideListDto>();

        foreach (var profile in profiles)
        {
            var user = await _userManager.FindByIdAsync(profile.UserId);
            if (user == null || user.IsDeleted) continue;

            var guideCities = await _unitOfWork.Repository<GuideCity>()
                .FindAsync(gc => gc.GuideProfileId == profile.Id);

            var langs = JsonSerializer.Deserialize<List<string>>(profile.LanguagesJson ?? "[]") ?? [];

            var cityNames = new List<string>();
            foreach (var gc in guideCities)
            {
                var city = await _unitOfWork.Repository<City>().GetByIdAsync(gc.CityId);
                if (city != null) cityNames.Add(city.NameEn);
            }

            result.Add(MapToListDto(user, profile, langs, cityNames));
        }

        return result;
    }

    public async Task ApproveGuideAsync(string guideId)
    {
        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideId)
            ?? throw new NotFoundException("Guide not found");

        var user = await _userManager.FindByIdAsync(guideId)
            ?? throw new NotFoundException("User not found");

        profile.IsApproved = true;
        _unitOfWork.Repository<GuideProfile>().Update(profile);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendGuideApprovalEmailAsync(user.Email!, user.FullName);
    }

    public async Task RejectGuideAsync(string guideId, string reason)
    {
        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideId)
            ?? throw new NotFoundException("Guide not found");

        var user = await _userManager.FindByIdAsync(guideId)
            ?? throw new NotFoundException("User not found");

        profile.RejectionReason = reason;
        _unitOfWork.Repository<GuideProfile>().Update(profile);
        await _unitOfWork.SaveChangesAsync();

        await _emailService.SendGuideRejectionEmailAsync(user.Email!, user.FullName, reason);
    }

    public async Task ToggleSuspendGuideAsync(string guideId)
    {
        var profile = await _unitOfWork.Repository<GuideProfile>()
            .FindOneAsync(g => g.UserId == guideId)
            ?? throw new NotFoundException("Guide not found");

        profile.IsSuspended = !profile.IsSuspended;
        _unitOfWork.Repository<GuideProfile>().Update(profile);
        await _unitOfWork.SaveChangesAsync();
    }

    // ─── Helpers ───────────────────────────────────────────────

    private static GuideProfileDto MapToProfileDto(ApplicationUser user, GuideProfile profile, List<string> cityNames)
    {
        var languages = JsonSerializer.Deserialize<List<string>>(profile.LanguagesJson ?? "[]") ?? [];

        return new GuideProfileDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            Email = user.Email!,
            AvatarUrl = user.AvatarUrl,
            Bio = profile.Bio,
            Languages = languages,
            ExperienceYears = profile.ExperienceYears,
            AverageRating = profile.AverageRating,
            TotalReviews = profile.TotalReviews,
            IsApproved = profile.IsApproved,
            IsAvailable = profile.IsAvailable,
            CoveredCities = cityNames
        };
    }

    private static GuideListDto MapToListDto(ApplicationUser user, GuideProfile profile, List<string> langs, List<string> cityNames)
    {
        return new GuideListDto
        {
            UserId = user.Id,
            FullName = user.FullName,
            AvatarUrl = user.AvatarUrl,
            AverageRating = profile.AverageRating,
            TotalReviews = profile.TotalReviews,
            ExperienceYears = profile.ExperienceYears,
            Languages = langs,
            CoveredCities = cityNames,
            IsAvailable = profile.IsAvailable
        };
    }
}