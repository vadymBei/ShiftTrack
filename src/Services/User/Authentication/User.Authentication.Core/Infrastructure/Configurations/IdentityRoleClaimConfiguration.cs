using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Core.Infrastructure.Configurations
{
    public class IdentityRoleClaimConfig : IEntityTypeConfiguration<IdentityRoleClaim<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
        {
            builder.ToTable("RoleClaims");

            builder.Property(c => c.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.RoleId)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
