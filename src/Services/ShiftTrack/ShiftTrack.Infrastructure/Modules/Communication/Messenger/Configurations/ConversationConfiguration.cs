using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Modules.Communication.Messenger.Entities;
using ShiftTrack.Infrastructure.Common.Configurations;

namespace ShiftTrack.Infrastructure.Modules.Communication.Messenger.Configurations;

public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
{
    public void Configure(EntityTypeBuilder<Conversation> builder)
    {
        builder.ToTable("Conversations");

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Title)
            .HasMaxLength(100);

        builder.Property(x => x.DirectKey)
            .HasMaxLength(50);

        builder.HasIndex(x => x.DirectKey)
            .IsUnique()
            .HasFilter("\"DirectKey\" IS NOT NULL");

        builder.HasIndex(x => x.LastMessageAt);
    }
}
