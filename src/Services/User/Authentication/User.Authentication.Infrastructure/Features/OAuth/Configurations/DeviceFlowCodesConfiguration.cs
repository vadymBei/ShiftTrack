using Duende.IdentityServer.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace User.Authentication.Infrastructure.Features.OAuth.Configurations;

public class DeviceFlowCodesConfiguration : IEntityTypeConfiguration<DeviceFlowCodes>
{
    public void Configure(EntityTypeBuilder<DeviceFlowCodes> builder)
    {
        builder.ToTable("DeviceCodes");
    }
}