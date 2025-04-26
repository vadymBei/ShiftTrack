using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Core.Domain.System.User.EmployeeRoles.Entities;

namespace ShiftTrack.Core.Infrastructure.Configurations.System.User.EmployeeRoles;

public class EmployeeRoleUnitConfiguration : IEntityTypeConfiguration<EmployeeRoleUnit>
{
    public void Configure(EntityTypeBuilder<EmployeeRoleUnit> builder)
    {
        builder.ToTable("EmployeeRoleUnits");
        
        builder
            .HasOne(e => e.EmployeeRole)
            .WithMany(e => e.Units)
            .HasForeignKey(e => e.EmployeeRoleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}