using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Rating)
            .IsRequired();

        builder.HasOne(r => r.Tourist)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.TouristId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.GuideProfile)
            .WithMany(g => g.Reviews)
            .HasForeignKey(r => r.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Booking)
            .WithOne(b => b.Review)
            .HasForeignKey<Review>(r => r.BookingId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}