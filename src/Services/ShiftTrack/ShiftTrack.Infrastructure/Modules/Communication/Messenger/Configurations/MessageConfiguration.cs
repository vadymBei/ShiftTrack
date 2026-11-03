using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Configurations;

public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Text)
            .HasMaxLength(4000);

        builder.HasOne(x => x.Conversation)
            .WithMany()
            .HasForeignKey(x => x.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ReplyToMessage)
            .WithMany()
            .HasForeignKey(x => x.ReplyToMessageId)
            .OnDelete(DeleteBehavior.SetNull);

        // Pages of a conversation are read by id.
        builder.HasIndex(x => new { x.ConversationId, x.Id });

        builder.HasIndex(x => new { x.AuthorId, x.ClientMessageId })
            .IsUnique()
            .HasFilter("\"ClientMessageId\" IS NOT NULL");
    }
}
