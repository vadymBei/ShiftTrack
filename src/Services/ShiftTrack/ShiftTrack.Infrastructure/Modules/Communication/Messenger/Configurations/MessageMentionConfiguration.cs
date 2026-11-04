using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Configurations;

public class MessageMentionConfiguration : IEntityTypeConfiguration<MessageMention>
{
    public void Configure(EntityTypeBuilder<MessageMention> builder)
    {
        builder.ToTable("MessageMentions");

        builder.HasKey(x => new { x.MessageId, x.EmployeeId });

        builder.HasOne(x => x.Message)
            .WithMany(x => x.Mentions)
            .HasForeignKey(x => x.MessageId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(x => !x.Employee.IsDeleted);

        // Unread mentions of an employee.
        builder.HasIndex(x => new { x.EmployeeId, x.MessageId });
    }
}
