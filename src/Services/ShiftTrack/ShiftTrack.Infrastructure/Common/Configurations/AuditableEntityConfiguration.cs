using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShiftTrack.Domain.Common.Abstractions;

namespace ShiftTrack.Infrastructure.Common.Configurations;

public static class AuditableEntityConfiguration
{
    public static void ConfigureAuditableEntity<T>(this EntityTypeBuilder<T> builder) where T : AuditableEntity
    {
        builder.HasOne(x => x.Author)
            .WithMany()
            .HasForeignKey(x => x.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Modifier)
            .WithMany()
            .HasForeignKey(x => x.ModifierId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
