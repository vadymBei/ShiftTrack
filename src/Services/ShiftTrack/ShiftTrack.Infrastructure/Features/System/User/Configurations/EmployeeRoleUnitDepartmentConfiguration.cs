using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Features.System.User.Configurations;

public class EmployeeRoleUnitDepartmentConfiguration : IEntityTypeConfiguration<EmployeeRoleUnitDepartment>
{
    public void Configure(EntityTypeBuilder<EmployeeRoleUnitDepartment> builder)
    {
        builder.ToTable("EmployeeRoleUnitDepartments");
        
        builder.ConfigureAuditableEntity();

        builder
            .HasOne(e => e.EmployeeRoleUnit)
            .WithMany(e => e.Departments)
            .HasForeignKey(e => e.EmployeeRoleUnitId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}