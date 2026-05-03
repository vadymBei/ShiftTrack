using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<LocationEntity> Locations { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}