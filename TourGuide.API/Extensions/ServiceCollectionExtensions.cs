using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TourGuide.Application.Interfaces;
using TourGuide.Application.Services;
using TourGuide.Domain.Entities;
using TourGuide.Infrastructure.Data;
using TourGuide.Infrastructure.Identity;
using TourGuide.Infrastructure.Services;

namespace TourGuide.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
        return services;
    }

    public static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = false;
            options.User.RequireUniqueEmail = true;
            options.SignIn.RequireConfirmedEmail = true;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddSettings(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<JwtSettings>(config.GetSection("JWT"));
        services.Configure<EmailSettings>(config.GetSection("Email"));
        services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
        services.Configure<PaymobSettings>(config.GetSection("Paymob"));
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ICloudinaryService, CloudinaryService>();
        services.AddScoped<INotificationPushService, NotificationPushService>();
        services.AddScoped<PaymobService>();
        services.AddHttpClient<PaymobService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(TourGuide.Domain.Interfaces.IRepository<>),
            typeof(TourGuide.Infrastructure.Repositories.GenericRepository<>));
        services.AddScoped<TourGuide.Domain.Interfaces.IUnitOfWork,
            TourGuide.Infrastructure.Repositories.UnitOfWork>();
        return services;
    }

    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, TourGuide.Application.Services.AuthService>();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, TourGuide.Application.Services.UserService>();
        services.AddScoped<ICityService, TourGuide.Application.Services.CityService>();
        services.AddScoped<ILandmarkService, TourGuide.Application.Services.LandmarkService>();
        services.AddScoped<IGuideService, TourGuide.Application.Services.GuideService>();
        services.AddScoped<IPackageService, TourGuide.Application.Services.PackageService>();
        services.AddScoped<ICustomTripService, CustomTripService>();
        services.AddScoped<IBookingService, BookingService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IPaymobService, PaymobService>();
        services.AddScoped<IChatService, ChatService>();
        services.AddScoped<IReviewService, ReviewService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }
}