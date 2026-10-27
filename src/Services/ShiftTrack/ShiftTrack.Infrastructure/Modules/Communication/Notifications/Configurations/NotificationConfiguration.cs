using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Communication.Notifications.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Communication.Notifications.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.Message)
            .HasMaxLength(1000);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(x => !x.Employee.IsDeleted);

        builder.HasIndex(x => new { x.EmployeeId, x.ReadAt, x.CreatedAt });

        builder.HasIndex(x => new { x.ModuleType, x.EntityId });
    }
}
