using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Booking.Vacations.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Booking.Vacations.Configurations;

public class VacationAccrualConfiguration : IEntityTypeConfiguration<VacationAccrual>
{
    public void Configure(EntityTypeBuilder<VacationAccrual> builder)
    {
        builder.ToTable("VacationAccruals");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Days)
            .HasPrecision(6, 2);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.Employee.IsDeleted);

        builder.HasIndex(x => new { x.EmployeeId, x.WorkingYear })
            .IsUnique();
    }
}
