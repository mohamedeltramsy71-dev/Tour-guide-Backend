using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class GuideCityConfiguration : IEntityTypeConfiguration<GuideCity>
{
    public void Configure(EntityTypeBuilder<GuideCity> builder)
    {
        builder.HasKey(gc => new { gc.GuideProfileId, gc.CityId });

        builder.HasOne(gc => gc.GuideProfile)
            .WithMany(g => g.CoveredCities)
            .HasForeignKey(gc => gc.GuideProfileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(gc => gc.City)
            .WithMany(c => c.GuideCities)
            .HasForeignKey(gc => gc.CityId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}