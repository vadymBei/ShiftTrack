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
    }
}