using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Core.Domain.Organization.Timesheet.Shifts.Entities;

namespace ShiftTrack.Core.Infrastructure.Configurations.Organization.Timesheet.Shifts
{
    public class ShiftConfiguration : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> builder)
        {
            builder.ToTable("Shifts");

            builder.HasIndex(x => x.IsDeleted);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
