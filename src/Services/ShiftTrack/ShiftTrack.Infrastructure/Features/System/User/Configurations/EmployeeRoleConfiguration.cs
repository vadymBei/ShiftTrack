using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.System.User.EmployeeRoles.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Features.System.User.Configurations;

public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
{
    public void Configure(EntityTypeBuilder<EmployeeRole> builder)
    {
        builder.ToTable("EmployeeRoles");

        builder.ConfigureAuditableEntity();
    }
}