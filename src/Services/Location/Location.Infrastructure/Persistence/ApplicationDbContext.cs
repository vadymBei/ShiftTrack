using Location.Application.Common.Interfaces;
using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<LocationEntity> Locations { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}