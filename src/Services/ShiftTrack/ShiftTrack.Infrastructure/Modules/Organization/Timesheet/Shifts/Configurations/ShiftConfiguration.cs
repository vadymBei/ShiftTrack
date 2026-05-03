using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Organization.Timesheet.Shifts.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Organization.Timesheet.Shifts.Configurations;

public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
{
    public void Configure(EntityTypeBuilder<Shift> builder)
    {
        builder.ToTable("Shifts");

        builder.ConfigureAuditableEntity();

        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}