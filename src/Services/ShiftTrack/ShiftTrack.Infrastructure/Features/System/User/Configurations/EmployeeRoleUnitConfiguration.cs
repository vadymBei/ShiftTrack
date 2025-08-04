using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Features.System.User.Configurations;

public class EmployeeRoleUnitConfiguration : IEntityTypeConfiguration<EmployeeRoleUnit>
{
    public void Configure(EntityTypeBuilder<EmployeeRoleUnit> builder)
    {
        builder.ToTable("EmployeeRoleUnits");
        
        builder.ConfigureAuditableEntity();

        builder
            .HasOne(e => e.EmployeeRole)
            .WithMany(e => e.Units)
            .HasForeignKey(e => e.EmployeeRoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}