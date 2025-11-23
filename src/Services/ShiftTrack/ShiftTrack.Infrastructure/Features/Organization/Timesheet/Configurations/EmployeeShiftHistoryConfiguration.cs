using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Features.Organization.Timesheet.Configurations;

public class EmployeeShiftHistoryConfiguration : IEntityTypeConfiguration<EmployeeShiftHistory>
{
    public void Configure(EntityTypeBuilder<EmployeeShiftHistory> builder)
    {
        builder.ToTable("EmployeeShiftHistory");

        builder.ConfigureAuditableEntity();

        builder.HasOne(x => x.PreviousShift)
            .WithMany()
            .HasForeignKey(x => x.PreviousShiftId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.NewShift)
            .WithMany()
            .HasForeignKey(x => x.NewShiftId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}