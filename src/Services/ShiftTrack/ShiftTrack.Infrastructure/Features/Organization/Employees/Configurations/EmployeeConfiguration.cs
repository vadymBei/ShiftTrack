using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Features.System.User.Employees.Entities;

namespace ShiftTrack.Infrastructure.Features.Organization.Employees.Configurations;

public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasIndex(c => c.Email)
            .IsUnique();

        builder.HasIndex(x => x.PhoneNumber)
            .IsUnique();

        builder.HasIndex(x => x.IsDeleted);

        builder.HasQueryFilter(x => !x.IsDeleted);

        builder.Property(t => t.Email)
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(t => t.Name)
            .HasMaxLength(64);

        builder.Property(t => t.Surname)
            .HasMaxLength(64);

        builder.Property(t => t.Patronymic)
            .HasMaxLength(64);

        builder.Property(t => t.PhoneNumber)
            .HasMaxLength(13);
    }
}