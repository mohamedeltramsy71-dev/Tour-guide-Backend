using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TourGuide.Domain.Entities;

namespace TourGuide.Infrastructure.Data.Configurations;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(b => b.Status)
            .HasConversion<string>();

        builder.Property(b => b.PaymentStatus)
            .HasConversion<string>();

        builder.HasOne(b => b.Tourist)
            .WithMany(u => u.Bookings)
            .HasForeignKey(b => b.TouristId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.GuideProfile)
            .WithMany(g => g.Bookings)
            .HasForeignKey(b => b.GuideProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Package)
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PackageId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}