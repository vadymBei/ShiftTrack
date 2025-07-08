using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Core.Domain.Booking.Vacations.Entities;

namespace ShiftTrack.Core.Infrastructure.Configurations.Booking.Vacations;

public class VacationConfiguration : IEntityTypeConfiguration<Vacation>
{
    public void Configure(EntityTypeBuilder<Vacation> builder)
    {
        builder.ToTable("Vacations");

        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}