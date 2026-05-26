using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Booking.BusinessTrips.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Booking.BusinessTrips.Configurations;

public class BusinessTripConfiguration: IEntityTypeConfiguration<BusinessTrip>
{
    public void Configure(EntityTypeBuilder<BusinessTrip> builder)
    {
        builder.ToTable("BusinessTrips");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Description)
            .HasMaxLength(2048);

        builder.Property(x => x.EstimatedBudget)
            .HasPrecision(18, 2);

        builder.Property(x => x.Status)
            .HasConversion<string>()
            .HasMaxLength(32);
        
        builder.HasMany(x => x.Participants)
            .WithMany()
            .UsingEntity(x => x.ToTable("BusinessTripParticipants"));

        builder.HasMany(x => x.Locations)
            .WithOne(x => x.BusinessTrip)
            .HasForeignKey(x => x.BusinessTripId);
    }
}   