using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.Booking.Vacations.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Features.Booking.Vacations.Configurations;

public class VacationConfiguration : IEntityTypeConfiguration<Vacation>
{
    public void Configure(EntityTypeBuilder<Vacation> builder)
    {
        builder.ToTable("Vacations");

        builder.ConfigureAuditableEntity();
        
        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}