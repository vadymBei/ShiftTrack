using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Infrastructure.Features.OAuth.Configurations;

public class PersistedGrantConfiguration : IEntityTypeConfiguration<PersistedGrant>
{
    public void Configure(EntityTypeBuilder<PersistedGrant> builder)
    {
        builder.ToTable("PersistedGrants");
    }
}