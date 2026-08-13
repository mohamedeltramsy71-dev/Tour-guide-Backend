using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class LandmarkConfiguration : IEntityTypeConfiguration<Landmark>
{
    public void Configure(EntityTypeBuilder<Landmark> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.NameAr)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.NameEn)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(l => l.EntryFee)
            .HasPrecision(18, 2);

        builder.Property(l => l.Category)
            .HasConversion<string>();

        builder.HasQueryFilter(l => !l.IsDeleted);

        builder.HasOne(l => l.City)
            .WithMany(c => c.Landmarks)
            .HasForeignKey(l => l.CityId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(l => l.Images)
            .WithOne(i => i.Landmark)
            .HasForeignKey(i => i.LandmarkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}