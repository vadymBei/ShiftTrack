using Location.Application.Common.Interfaces;
using Location.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Location.Application.Common.Services;

public class LocationService(
    ILocationRepository locationRepository,
    IApplicationDbContext applicationDbContext) : ILocationService
{
    public async Task<IEnumerable<LocationEntity>> Search(string query, CancellationToken cancellationToken)
    {
        var locations = await applicationDbContext.Locations
            .AsNoTracking()
            .Where(x => EF.Functions.ILike(x.Name, $"%{query}%"))
            .ToListAsync(cancellationToken);

        if (locations.Count == 0)
        {
            locations = (await locationRepository.Search(query, cancellationToken))
                .GroupBy(x => x.IntegrationId)
                .Select(g => g.First())
                .ToList();
            
            applicationDbContext.Locations.AddRange(locations);
            await applicationDbContext.SaveChangesAsync(cancellationToken);
        }
        
        return locations;
    }
}