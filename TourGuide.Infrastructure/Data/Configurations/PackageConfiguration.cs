using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class PackageConfiguration : IEntityTypeConfiguration<Package>
{
    public void Configure(EntityTypeBuilder<Package> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.HasQueryFilter(p => !p.IsDeleted);

        builder.HasOne(p => p.City)
            .WithMany(c => c.Packages)
            .HasForeignKey(p => p.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.GuideProfile)
            .WithMany(g => g.Packages)
            .HasForeignKey(p => p.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Images)
            .WithOne(i => i.Package)
            .HasForeignKey(i => i.PackageId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}