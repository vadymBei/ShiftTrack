using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Core.Infrastructure.Configurations
{
    public class IdentityUserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.ToTable("UserRoles");

            builder.Property(c => c.RoleId)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.UserId)
                .HasMaxLength(64)
                .IsRequired();
        }
    }
}
