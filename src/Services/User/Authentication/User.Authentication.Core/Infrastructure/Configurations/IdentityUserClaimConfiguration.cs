using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Core.Infrastructure.Configurations
{
    public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
        {
            builder.ToTable("UsersClaims");

            builder.Property(c => c.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.UserId)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
