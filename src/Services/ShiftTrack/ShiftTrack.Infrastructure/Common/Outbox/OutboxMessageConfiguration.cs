using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Common.Outbox;

namespace ShiftTrack.Infrastructure.Common.Outbox;

public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        builder.ToTable("OutboxMessages");

        builder.Property(x => x.Type)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(x => x.Content)
            .HasColumnType("jsonb")
            .IsRequired();

        // Pending messages are the only ones the processor reads.
        builder.HasIndex(x => x.Id)
            .HasFilter("\"ProcessedAt\" IS NULL")
            .HasDatabaseName("IX_OutboxMessages_Pending");

        builder.HasIndex(x => x.ProcessedAt);
    }
}
