using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Core.Infrastructure.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.ToTable("Roles");

            builder.Property(c => c.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.Name)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(c => c.NormalizedName)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}
