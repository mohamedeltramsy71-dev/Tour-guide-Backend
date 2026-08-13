using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<GuideProfile> GuideProfiles => Set<GuideProfile>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<GuideCity> GuideCities => Set<GuideCity>();
    public DbSet<Landmark> Landmarks => Set<Landmark>();
    public DbSet<LandmarkImage> LandmarkImages => Set<LandmarkImage>();
    public DbSet<Package> Packages => Set<Package>();
    public DbSet<PackageImage> PackageImages => Set<PackageImage>();
    public DbSet<PackageLandmark> PackageLandmarks => Set<PackageLandmark>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}