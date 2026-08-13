using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class PackageLandmarkConfiguration : IEntityTypeConfiguration<PackageLandmark>
{
    public void Configure(EntityTypeBuilder<PackageLandmark> builder)
    {
        builder.HasKey(pl => new { pl.PackageId, pl.LandmarkId });

        builder.HasOne(pl => pl.Package)
            .WithMany(p => p.PackageLandmarks)
            .HasForeignKey(pl => pl.PackageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pl => pl.Landmark)
            .WithMany(l => l.PackageLandmarks)
            .HasForeignKey(pl => pl.LandmarkId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}