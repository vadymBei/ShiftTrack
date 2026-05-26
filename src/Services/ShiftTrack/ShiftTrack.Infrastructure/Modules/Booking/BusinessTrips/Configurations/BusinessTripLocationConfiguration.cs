using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Configurations;

public class BusinessTripLocationConfiguration : IEntityTypeConfiguration<BusinessTripLocation>
{
    public void Configure(EntityTypeBuilder<BusinessTripLocation> builder)
    {
        builder.ToTable("BusinessTripLocations");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.LocationIntegrationId)
            .HasMaxLength(128)
            .IsRequired();
    }
}