using Location.Application.Common.Interfaces;
using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Infrastructure.Repositories;

public class LocationRepository(
    IApplicationDbContext applicationDbContext) : ILocationRepository
{
    public async Task<IEnumerable<LocationEntity>> GetByIntegrationIds(IEnumerable<string> integrationIds,
        CancellationToken cancellationToken)
    {
        var locations = await applicationDbContext.Locations
            .AsNoTracking()
            .Where(x => integrationIds.Contains(x.IntegrationId))
            .ToListAsync(cancellationToken);

        locations = locations
            .DistinctBy(x => x.IntegrationId)
            .ToList();

        return locations;
    }

    public async Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken)
    {
        var locations = await applicationDbContext.Locations
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Name, $"%{query}%"))
            .ToListAsync(cancellationToken);

        locations = locations
            .DistinctBy(x => x.IntegrationId)
            .ToList();
        
        return locations;
    }

    public async Task<IEnumerable<LocationEntity>> Create(IEnumerable<LocationEntity> entities,
        CancellationToken cancellationToken)
    {
        applicationDbContext.Locations.AddRange(entities);
        await applicationDbContext.SaveChangesAsync(cancellationToken);

        return entities;
    }
}