using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Core.Infrastructure.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<ShiftTrack.Authentication.Models.User>
    {
        public void Configure(EntityTypeBuilder<ShiftTrack.Authentication.Models.User> builder)
        {
            builder.ToTable("Users");

            builder.Property(c => c.Id)
                .HasMaxLength(64)
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(128);

            builder.Property(c => c.NormalizedEmail)
                .HasMaxLength(128);

            builder.Property(c => c.UserName)
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(c => c.NormalizedUserName)
                .HasMaxLength(128)
                .IsRequired();
        }
    }
}
